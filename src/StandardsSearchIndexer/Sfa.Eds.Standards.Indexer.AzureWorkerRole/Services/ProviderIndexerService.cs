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
        private readonly IProviderHelper _providerHelper;
        private readonly IProviderIndexSettings _providerIndexSettings;

        public ProviderIndexerService(
            IProviderIndexSettings providerIndexSettings,
            IProviderHelper providerHelper)
        {
            _providerIndexSettings = providerIndexSettings;
            _providerHelper = providerHelper;
        }

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Log.Info("Creating new provider index...");

            var indexProperlyCreated = _providerHelper.CreateIndex(scheduledRefreshDateTime);
            if (!indexProperlyCreated)
            {
                Log.Info("Provider index not created properly, exiting...");
                return;
            }

            Log.Info("Indexing providers...");
            await _providerHelper.IndexProviders(scheduledRefreshDateTime).ConfigureAwait(false);

            PauseWhileIndexingIsBeingRun();

            if (_providerHelper.IsIndexCorrectlyCreated(scheduledRefreshDateTime))
            {
                Log.Info("Swapping provider indexes...");

                _providerHelper.SwapIndexes(scheduledRefreshDateTime);

                Log.Info("Swap completed...");

                Log.Info("Deleting old provider indexes...");

                _providerHelper.DeleteOldIndexes(scheduledRefreshDateTime);

                Log.Info("Deletion completed...");
            }
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _providerIndexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}