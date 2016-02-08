using System;
using System.Reflection;
using System.Threading;
using log4net;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public class StandardIndexerService : IStandardIndexerService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IStandardHelper _standardHelper;
        private readonly IStandardIndexSettings _standardIndexSettings;

        public StandardIndexerService(
            IStandardIndexSettings standardIndexSettings,
            IStandardHelper standardHelper)
        {
            _standardIndexSettings = standardIndexSettings;
            _standardHelper = standardHelper;
        }

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Log.Info("Creating new standard index...");

            var indexProperlyCreated = _standardHelper.CreateIndex(scheduledRefreshDateTime);
            if (!indexProperlyCreated)
            {
                Log.Info("Standard index not created properly, exiting...");
                return;
            }

            Log.Info("Indexing standard PDFs...");
            await _standardHelper.IndexStandards(scheduledRefreshDateTime).ConfigureAwait(false);

            PauseWhileIndexingIsBeingRun();

            if (_standardHelper.IsIndexCorrectlyCreated(scheduledRefreshDateTime))
            {
                Log.Info("Swapping standard indexes...");

                _standardHelper.SwapIndexes(scheduledRefreshDateTime);

                Log.Info("Swap completed...");

                Log.Info("Deleting old standard indexes...");

                _standardHelper.DeleteOldIndexes(scheduledRefreshDateTime);

                Log.Info("Deletion completed...");
            }
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _standardIndexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}