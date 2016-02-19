using System;
using System.Reflection;
using System.Threading;
using log4net;
using Sfa.Eds.Indexer.Settings.Settings;
using Sfa.Eds.Indexer.StandardIndexer.Services;
using Sfa.Eds.StandardIndexer.Helpers;

namespace Sfa.Eds.StandardIndexer.Services
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
                Log.Error("Standard index not created properly, exiting...");
                return;
            }

            Log.Info("Indexing standard PDFs...");
            var standards = await _standardHelper.GetStandardsFromAzureAsync();

            await _standardHelper.IndexStandards(scheduledRefreshDateTime, standards).ConfigureAwait(false);

            PauseWhileIndexingIsBeingRun();

            if (_standardHelper.IsIndexCorrectlyCreated(scheduledRefreshDateTime))
            {
                Log.Info("Standard index created, pointing alias to new index...");

                Log.Debug("Swapping standard indexes...");

                _standardHelper.SwapIndexes(scheduledRefreshDateTime);

                Log.Debug("Swap completed...");

                Log.Debug("Deleting old standard indexes...");

                _standardHelper.DeleteOldIndexes(scheduledRefreshDateTime);

                Log.Debug("Deletion completed...");
            }
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _standardIndexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}