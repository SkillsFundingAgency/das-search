using System;
using System.Reflection;
using System.Threading;
using log4net;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public class ProviderIndexerService : IProviderIndexerService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IStandardHelper _standardHelper;
        private readonly IProviderIndexSettings _providerIndexSettings;

        public ProviderIndexerService(
            IProviderIndexSettings providerIndexSettings,
            IStandardHelper standardHelper)
        {
            _providerIndexSettings = providerIndexSettings;
            _standardHelper = standardHelper;
        }

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            /*
            Log.Info("Creating new index...");

            var existingPreviousIndex = _standardHelper.CreateIndex(scheduledRefreshDateTime);
            if (existingPreviousIndex)
            {
                Log.Info("Index already exists, exiting...");
                return;
            }

            Log.Info("Indexing PDFs...");
            await _standardHelper.IndexStandards(scheduledRefreshDateTime).ConfigureAwait(false);

            PauseWhileIndexingIsBeingRun();

            if (_standardHelper.IsIndexCorrectlyCreated())
            {
                Log.Info("Swapping indexes...");

                _standardHelper.SwapIndexes(scheduledRefreshDateTime);

                Log.Info("Swap completed...");

                Log.Info("Deleting old indexes...");

                _standardHelper.DeleteOldIndexes(scheduledRefreshDateTime);

                Log.Info("Deletion completed...");
            }
            */
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _providerIndexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}