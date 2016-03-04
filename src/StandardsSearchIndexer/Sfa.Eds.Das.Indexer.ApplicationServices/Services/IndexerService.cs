namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Common.Settings;
    using Sfa.Eds.Das.Indexer.Core;

    public class IndexerService<T> : IIndexerService<T>
    {
        private readonly IGenericIndexerHelper<T> _indexerHelper;

        private readonly ILog Log;

        private readonly IIndexSettings<T> _indexSettings;

        private readonly string _name;

        public IndexerService(IIndexSettings<T> indexSettings, IGenericIndexerHelper<T> indexerHelper, ILog log)
        {
            _indexSettings = indexSettings;
            _indexerHelper = indexerHelper;
            Log = log;
            _name = typeof(T).Name;
        }

        public async Task CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            await Task.Run(
                () =>
                    {
                        Log.Info($"Creating new scheduled {_name} index at " + DateTime.Now);
                        try
                        {
                            var indexProperlyCreated = _indexerHelper.CreateIndex(scheduledRefreshDateTime);
                            if (!indexProperlyCreated)
                            {
                                Log.Error($"{_name} index not created properly, exiting...");
                                return;
                            }

                            Log.Info($"Indexing {_name}s...");
                            var entries = _indexerHelper.LoadEntries();
                            _indexerHelper.IndexEntries(scheduledRefreshDateTime, entries);

                            PauseWhileIndexingIsBeingRun();

                            if (_indexerHelper.IsIndexCorrectlyCreated(scheduledRefreshDateTime))
                            {
                                Log.Info($"{_name} index created, pointing alias to new index...");

                                Log.Debug($"Swapping {_name} indexes...");

                                _indexerHelper.SwapIndexes(scheduledRefreshDateTime);

                                Log.Debug("Swap completed...");

                                Log.Debug($"Deleting old {_name} indexes...");

                                _indexerHelper.DeleteOldIndexes(scheduledRefreshDateTime);

                                Log.Debug("Deletion completed...");
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            throw;
                        }
                    }).ConfigureAwait(false);
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _indexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}