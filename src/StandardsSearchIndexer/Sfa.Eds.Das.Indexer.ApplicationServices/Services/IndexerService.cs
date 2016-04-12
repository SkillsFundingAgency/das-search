namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public class IndexerService<T> : IIndexerService<T>
    {
        private readonly IGenericIndexerHelper<T> _indexerHelper;

        private readonly ILog _log;

        private readonly IIndexSettings<T> _indexSettings;

        private readonly string _name;

        public IndexerService(IIndexSettings<T> indexSettings, IGenericIndexerHelper<T> indexerHelper, ILog log)
        {
            _indexSettings = indexSettings;
            _indexerHelper = indexerHelper;
            _log = log;
            _name = typeof(T).Name;
        }

        public async Task CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            _log.Info($"Creating new scheduled {_name} index at " + DateTime.Now);

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var newIndexName = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _indexSettings.IndexesAlias);
                var indexProperlyCreated = _indexerHelper.CreateIndex(newIndexName);

                if (!indexProperlyCreated)
                {
                    _log.Error($"{_name} index not created properly, exiting...");
                    return;
                }

                _log.Info($"Indexing {_name}s...");

                await _indexerHelper.IndexEntries(newIndexName).ConfigureAwait(false);

                PauseWhileIndexingIsBeingRun();

                var indexHasBeenCreated = _indexerHelper.IsIndexCorrectlyCreated(newIndexName);

                if (indexHasBeenCreated)
                {
                    _indexerHelper.ChangeUnderlyingIndexForAlias(newIndexName);

                    _log.Debug("Swap completed...");

                    _indexerHelper.DeleteOldIndexes(scheduledRefreshDateTime);
                }

                stopwatch.Stop();
                var properties = new Dictionary<string, object>() { { "Alias", _indexSettings.IndexesAlias }, { "ExecutionTime", stopwatch.ElapsedMilliseconds }, { "IndexCorrectlyCreated", indexHasBeenCreated } };
                _log.Info("Elasticsearch.CreateScheduledIndex", properties);
                _log.Info($"{_name} Indexing complete.");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }
        }

        private void PauseWhileIndexingIsBeingRun()
        {
            var time = _indexSettings.PauseTime;
            Thread.Sleep(int.Parse(time));
        }
    }
}