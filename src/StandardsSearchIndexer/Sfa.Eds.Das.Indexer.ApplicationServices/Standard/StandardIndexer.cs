namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Services;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Services;
    using Settings;

    public sealed class StandardIndexer : IGenericIndexerHelper<MetaDataItem>
    {
        private readonly IMetaDataHelper _metaDataHelper;
        private readonly IMaintainSearchIndexes<MetaDataItem> _searchIndexMaintainer;
        private readonly IIndexSettings<MetaDataItem> _settings;
        private readonly ILog _log;

        public StandardIndexer(
            IIndexSettings<MetaDataItem> settings,
            IMaintainSearchIndexes<MetaDataItem> searchIndexMaintainer,
            IMetaDataHelper metaDataHelper,
            ILog log)
        {
            _settings = settings;
            _metaDataHelper = metaDataHelper;
            _searchIndexMaintainer = searchIndexMaintainer;
            _log = log;
        }

        public async Task IndexEntries(string indexName, ICollection<MetaDataItem> entries)
        {
            try
            {
                _log.Debug("Indexing " + entries.Count + " standards");

                await _searchIndexMaintainer.IndexEntries(indexName, entries).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.Error("Error indexing Standards", ex);
            }
        }

        public Task<ICollection<MetaDataItem>> LoadEntries()
        {
            _metaDataHelper.UpdateMetadataRepository();
            _log.Info("Indexing standard PDFs...");

            var standardsMetaData = _metaDataHelper.GetAllStandardsMetaData();
            return Task.FromResult<ICollection<MetaDataItem>>(standardsMetaData.ToList());
        }

        public bool CreateIndex(string indexName)
        {
            var indexExistsResponse = _searchIndexMaintainer.IndexExists(indexName);

            // If it already exists and is empty, let's delete it.
            if (indexExistsResponse)
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

        public void SwapIndexes(string newIndexName)
        {
            if(!_searchIndexMaintainer.AliasExists(_settings.IndexesAlias))
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
    }
}