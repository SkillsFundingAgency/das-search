using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Nest;
using Newtonsoft.Json;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public class StandardService : IStandardService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IStandardIndexSettings _standardIndexSettings;
        private readonly IStandardHelper _standardHelper;

        public StandardService(
            IStandardIndexSettings standardIndexSettings,
            IStandardHelper standardHelper)
        {
            _standardIndexSettings = standardIndexSettings;
            _standardHelper = standardHelper;
        }

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Log.Info("Creating new index...");

            var existingPreviousIndex = _standardHelper.CreateIndex(scheduledRefreshDateTime);
            if (existingPreviousIndex)
            {
                Log.Info("Index already exists, exiting...");
                return;
            }

            await _standardHelper.IndexStandards(scheduledRefreshDateTime);

            PauseWhileIndexingIsBeingRun();

            if (_standardHelper.IsIndexCorrectlyCreated())
            {
                Log.Info("Swapping indexes...");

                _standardHelper.SwapIndexes(scheduledRefreshDateTime);
            }
        }
        
        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _standardIndexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}