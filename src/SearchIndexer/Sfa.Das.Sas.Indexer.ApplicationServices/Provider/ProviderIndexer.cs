using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Provider
{
    public sealed class ProviderIndexer : IGenericIndexerHelper<IMaintainProviderIndex>
    {
        private readonly IGetActiveProviders _activeProviderClient;

        private readonly IGetApprenticeshipProviders _providerRepository;
        private readonly IMaintainProviderIndex _searchIndexMaintainer;
        private readonly IIndexSettings<IMaintainProviderIndex> _settings;

        private readonly IProviderFeatures _features;

        private readonly ILog _log;

        public ProviderIndexer(
            IIndexSettings<IMaintainProviderIndex> settings,
            IMaintainProviderIndex searchIndexMaintainer,
            IProviderFeatures features,
            IGetApprenticeshipProviders providerRepository,
            IGetActiveProviders activeProviderClient,
            ILog log)
        {
            _settings = settings;
            _features = features;
            _providerRepository = providerRepository;
            _activeProviderClient = activeProviderClient;
            _searchIndexMaintainer = searchIndexMaintainer;
            _log = log;
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

            return _searchIndexMaintainer.IndexExists(indexName);
        }

        public async Task IndexEntries(string indexName)
        {
            var entries = await LoadEntries();
            try
            {
                _log.Debug("Indexing " + entries.Count + " providers");

                await _searchIndexMaintainer.IndexEntries(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error indexing provider");
                throw;
            }
        }

        public bool IsIndexCorrectlyCreated(string indexName)
        {
            return _searchIndexMaintainer.IndexContainsDocuments(indexName);
        }

        // TODO: LWA - The argument seems a little strange to this method.
        public void ChangeUnderlyingIndexForAlias(string newIndexName)
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
            var oneDayAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-2), _settings.IndexesAlias, "yyyy-MM-dd");
            var twoDaysAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-3), _settings.IndexesAlias, "yyyy-MM-dd");

            return _searchIndexMaintainer.DeleteIndexes(x => x.StartsWith(oneDayAgo2) || x.StartsWith(twoDaysAgo2));
        }

        public async Task<ICollection<Core.Models.Provider.Provider>> LoadEntries()
        {
            var providers = await _providerRepository.GetApprenticeshipProvidersAsync();
            if (_features.FilterInactiveProviders)
            {
                var activeProviders = _activeProviderClient.GetActiveProviders().ToList();

                return providers.Where(x => activeProviders.Contains(x.Ukprn)).ToList();
            }

            return providers.ToList();
        }
    }
}