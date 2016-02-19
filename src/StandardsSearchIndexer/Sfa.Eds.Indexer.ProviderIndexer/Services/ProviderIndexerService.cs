using System;
using System.Reflection;
using System.Threading;
using log4net;
using Sfa.Eds.Das.Indexer.Settings.Settings;
using Sfa.Eds.Das.ProviderIndexer.Helpers;

namespace Sfa.Eds.Das.ProviderIndexer.Services
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

        public void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Log.Info("Creating new provider index...");
            try
            {
                var indexProperlyCreated = _providerHelper.CreateIndex(scheduledRefreshDateTime);
                if (!indexProperlyCreated)
                {
                    Log.Error("Provider index not created properly, exiting...");
                    return;
                }

                Log.Info("Indexing providers...");
                var providers = _providerHelper.GetProviders();

                _providerHelper.IndexProviders(scheduledRefreshDateTime, providers);

                PauseWhileIndexingIsBeingRun();

                if (_providerHelper.IsIndexCorrectlyCreated(scheduledRefreshDateTime))
                {
                    Log.Info("Provider index created, pointing alias to new index...");

                    Log.Debug("Swapping provider indexes...");

                    _providerHelper.SwapIndexes(scheduledRefreshDateTime);

                    Log.Debug("Swap completed...");

                    Log.Debug("Deleting old provider indexes...");

                    _providerHelper.DeleteOldIndexes(scheduledRefreshDateTime);

                    Log.Debug("Deletion completed...");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _providerIndexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}