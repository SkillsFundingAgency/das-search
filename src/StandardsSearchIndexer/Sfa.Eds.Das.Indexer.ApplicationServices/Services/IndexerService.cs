using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.Core;
using Sfa.Eds.Das.Indexer.Core.Services;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces
{
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
                async () =>
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
                            var entries = await _indexerHelper.LoadEntries();
                            await _indexerHelper.IndexEntries(scheduledRefreshDateTime, entries);

                            PauseWhileIndexingIsBeingRun();

                            if (_indexerHelper.IsIndexCorrectlyCreated(scheduledRefreshDateTime))
                            {
                                _indexerHelper.SwapIndexes(scheduledRefreshDateTime);

                                Log.Debug("Swap completed...");

                                _indexerHelper.DeleteOldIndexes(scheduledRefreshDateTime);
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