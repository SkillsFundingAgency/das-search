using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Services
{
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
            _name = typeof(T) == typeof(IMaintainProviderIndex) ? "Provider Index" : "Apprenticeship Index";
        }

        public async Task CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            _log.Info($"Creating new scheduled {_name}");

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

                _log.Info($"Indexing documents for {_name}.");

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
                var properties = new Dictionary<string, object> { { "Alias", _indexSettings.IndexesAlias }, { "ExecutionTime", stopwatch.ElapsedMilliseconds }, { "IndexCorrectlyCreated", indexHasBeenCreated } };
                _log.Debug($"Created {_name}", properties);
                _log.Info($"{_name}ing complete.");
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