namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Services;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public sealed class ApprenticeshipIndexer : IGenericIndexerHelper<IMaintainApprenticeshipIndex>
    {
        private readonly IIndexSettings<IMaintainApprenticeshipIndex> _settings;
        private readonly IMaintainApprenticeshipIndex _searchIndexMaintainer;
        private readonly IMetaDataHelper _metaDataHelper;
        private readonly ILog _log;

        public ApprenticeshipIndexer(
            IIndexSettings<IMaintainApprenticeshipIndex> settings,
            IMaintainApprenticeshipIndex searchIndexMaintainer,
            IMetaDataHelper metaDataHelper,
            ILog log)
        {
            _settings = settings;
            _searchIndexMaintainer = searchIndexMaintainer;
            _metaDataHelper = metaDataHelper;
            _log = log;
        }

        public async Task IndexEntries(string indexName)
        {
            _metaDataHelper.UpdateMetadataRepository();

            await IndexStandards(indexName).ConfigureAwait(false);
            await IndexFrameworks(indexName).ConfigureAwait(false);
        }

        public bool CreateIndex(string indexName)
        {
            // If it already exists and is empty, let's delete it.
            if (_searchIndexMaintainer.IndexExists(indexName))
            {
                _log.Warn("Index already exists, deleting and creating a new one");

                _searchIndexMaintainer.DeleteIndex(indexName);
            }

            // create index
            _searchIndexMaintainer.CreateIndex(indexName);

            return _searchIndexMaintainer.IndexExists(indexName);
        }

        public bool IsIndexCorrectlyCreated(string indexName)
        {
            return _searchIndexMaintainer.IndexContainsDocuments(indexName);
        }

        public void ChangeUnderlyingIndexForAlias(string newIndexName)
        {
            if (!_searchIndexMaintainer.AliasExists(_settings.IndexesAlias))
            {
                _log.Warn("Alias doesn't exists, creating a new one...");

                _searchIndexMaintainer.CreateIndexAlias(_settings.IndexesAlias, newIndexName);
            }

            _searchIndexMaintainer.SwapAliasIndex(_settings.IndexesAlias, newIndexName);
        }

        public bool DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var oneDayAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-1), _settings.IndexesAlias, "yyyy-MM-dd");
            var twoDaysAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-2), _settings.IndexesAlias, "yyyy-MM-dd");

            return _searchIndexMaintainer.DeleteIndexes(x => x.StartsWith(oneDayAgo2) || x.StartsWith(twoDaysAgo2));
        }

        private async Task IndexStandards(string indexName)
        {
            var entries = await LoadStandardMetaData();
            try
            {
                _log.Debug("Indexing " + entries.Count + " standards");

                await _searchIndexMaintainer.IndexStandards(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Error("Error indexing Standards", ex);
            }
        }

        private async Task IndexFrameworks(string indexName)
        {
            var entries = _metaDataHelper.GetAllFrameworkMetaData();
            try
            {
                _log.Debug("Indexing " + entries.Count + " frameworks");

                await _searchIndexMaintainer.IndexFrameworks(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Error("Error indexing Frameworks", ex);
            }
        }

        private Task<ICollection<StandardMetaData>> LoadStandardMetaData()
        {
            _log.Info("Loading standard meta data ...");

            var standardsMetaData = _metaDataHelper.GetAllStandardsMetaData();
            return Task.FromResult<ICollection<StandardMetaData>>(standardsMetaData.ToList());
        }
    }
}