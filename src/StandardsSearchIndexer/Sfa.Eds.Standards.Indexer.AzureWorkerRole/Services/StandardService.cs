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
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IStandardIndexSettings _standardIndexSettings;
        private readonly IStandardHelper _standardHelper;

        public StandardService(
            IElasticsearchClientFactory elasticsearchClientFactory,
            IStandardIndexSettings standardIndexSettings,
            IStandardHelper standardHelper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _standardIndexSettings = standardIndexSettings;
            _standardHelper = standardHelper;
        }

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            _log.Info("Creating new index...");

            var existingPreviousIndex = _standardHelper.CreateIndex(scheduledRefreshDateTime);
            if (existingPreviousIndex)
            {
                _log.Info("Index already exists, exiting...");
                return;
            }

            await _standardHelper.IndexStandards(scheduledRefreshDateTime);

            PauseWhileIndexingIsBeingRun();

            if (_standardHelper.IsIndexCorrectlyCreated())
            {
                _log.Info("Swapping indexes...");

                _standardHelper.SwapIndexes(scheduledRefreshDateTime);
            }
        }
        
        private void PauseWhileIndexingIsBeingRun()
        {
            Thread.Sleep(int.Parse(_standardIndexSettings.PauseTime));
        }
    }
}