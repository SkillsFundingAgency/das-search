using System;
using System.Threading;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.Core;
using Sfa.Eds.Das.Indexer.Core.Services;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces
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
            _name = typeof(T).Name;
        }

        public async Task CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            _log.Info($"Creating new scheduled {_name} index at " + DateTime.Now);

            try
            {
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

                if (_indexerHelper.IsIndexCorrectlyCreated(newIndexName))
                {
                    _indexerHelper.SwapIndexes(newIndexName);

                    _log.Debug("Swap completed...");

                    _indexerHelper.DeleteOldIndexes(scheduledRefreshDateTime);
                }

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