using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.Core;
using Sfa.Eds.Das.Indexer.Core.Services;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Provider
{
    public sealed class ProviderIndexer : IGenericIndexerHelper<Core.Models.Provider.Provider>
    {
        private readonly IGetActiveProviders _activeProviderClient;
        private readonly IGetApprenticeshipProviders _providerRepository;
        private readonly IMaintainSearchIndexes<Core.Models.Provider.Provider> _searchIndexMaintainer;
        private readonly IIndexSettings<Core.Models.Provider.Provider> _settings;
        private readonly ILog _log;

        public ProviderIndexer(
            IIndexSettings<Core.Models.Provider.Provider> settings,
            IMaintainSearchIndexes<Core.Models.Provider.Provider> searchIndexMaintainer,
            IGetApprenticeshipProviders providerRepository,
            IGetActiveProviders activeProviderClient,
            ILog log)
        {
            _settings = settings;
            _providerRepository = providerRepository;
            _activeProviderClient = activeProviderClient;
            _searchIndexMaintainer = searchIndexMaintainer;
            _log = log;
        }

        public async Task<ICollection<Core.Models.Provider.Provider>> LoadEntries()
        {
            var providers = await _providerRepository.GetApprenticeshipProvidersAsync();

            var activeProviders = _activeProviderClient.GetActiveProviders().ToList();

            return providers.Where(x => activeProviders.Contains(x.Ukprn)).ToList();
        }

        public bool CreateIndex(string indexName)
        {
            var indexExists = _searchIndexMaintainer.IndexExists(indexName);

            // If it already exists and is empty, let's delete it.
            if (indexExists)
            {
                _log.Warn("Index already exists, deleting and creating a new one");

                _searchIndexMaintainer.DeleteIndex(indexName);
            }

            _searchIndexMaintainer.CreateIndex(indexName);

            var exists = _searchIndexMaintainer.IndexExists(indexName);

            return exists;
        }

        public async Task IndexEntries(string indexName, ICollection<Core.Models.Provider.Provider> entries)
        {
            try
            {
                _log.Debug("Indexing " + entries.Count + " providers");

                await _searchIndexMaintainer.IndexEntries(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Error("Error indexing providers: " + ex.Message, ex);
                throw;
            }
        }

        public bool IsIndexCorrectlyCreated(string indexName)
        {
            return _searchIndexMaintainer.IndexContainsDocuments(indexName);
        }

        // TODO: LWA - The argusment seems a little strange to this method.
        public void SwapIndexes(string newIndexName)
        {
            if (!_searchIndexMaintainer.AliasExists(_settings.IndexesAlias))
            {
                _log.Warn("Alias doesn't exists, creating a new one...");

                _searchIndexMaintainer.CreateIndexAlias(_settings.IndexesAlias, newIndexName);
            }
            else
            {
                _searchIndexMaintainer.SwapAliasIndex(_settings.IndexesAlias, newIndexName);
            }
        }

        public bool DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var oneDayAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-1), _settings.IndexesAlias, "yyyy-MM-dd");
            var twoDaysAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-2), _settings.IndexesAlias, "yyyy-MM-dd");

            return _searchIndexMaintainer.DeleteIndexes(x => x.StartsWith(oneDayAgo2) || x.StartsWith(twoDaysAgo2));
        }
    }
}