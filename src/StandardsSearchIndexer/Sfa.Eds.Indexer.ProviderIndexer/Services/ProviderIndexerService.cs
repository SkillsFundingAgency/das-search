using System;
using System.Reflection;
using System.Threading;
using log4net;
using Sfa.Eds.Das.ProviderIndexer.Helpers;
using Sfa.Eds.Das.ProviderIndexer.Settings;

namespace Sfa.Eds.Das.ProviderIndexer.Services
{
    using System.Linq;

    using Sfa.Eds.Das.ProviderIndexer.Clients;

    public class ProviderIndexerService : IProviderIndexerService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IProviderHelper _providerHelper;
        private readonly ICourseDirectoryClient _courseDirectoryClient;

        private readonly IActiveProviderClient _activeProviderClient;

        private readonly IProviderIndexSettings _providerIndexSettings;

        public ProviderIndexerService(
            IProviderIndexSettings providerIndexSettings,
            IProviderHelper providerHelper,
            ICourseDirectoryClient courseDirectoryClient,
            IActiveProviderClient activeProviderClient)
        {
            _providerIndexSettings = providerIndexSettings;
            _providerHelper = providerHelper;
            _courseDirectoryClient = courseDirectoryClient;
            _activeProviderClient = activeProviderClient;
        }

        public async void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Log.Info("Creating new scheduled provider index at " + DateTime.Now);
            try
            {
                var indexProperlyCreated = _providerHelper.CreateIndex(scheduledRefreshDateTime);
                if (!indexProperlyCreated)
                {
                    Log.Error("Provider index not created properly, exiting...");
                    return;
                }

                Log.Info("Indexing providers...");
                var providers = await _courseDirectoryClient.GetProviders();
                var activeProviders = await _activeProviderClient.GetProviders();

                _providerHelper.IndexProviders(scheduledRefreshDateTime, providers.Where(x => activeProviders.Contains(x.UkPrn)).ToList());

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