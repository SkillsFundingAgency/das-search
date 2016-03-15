namespace Sfa.Eds.Das.Indexer.ApplicationServices.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Core;
    using Core.Models.Framework;

    using Services;
    using Settings;
    using Core.Services;

    public sealed class FrameworkIndexer : IGenericIndexerHelper<FrameworkMetaData>
    {
        private readonly IIndexSettings<FrameworkMetaData> _settings;

        private readonly ILog _log;

        private readonly IMaintainSearchIndexes<FrameworkMetaData> _searchIndexMaintainer;

        public FrameworkIndexer(IIndexSettings<FrameworkMetaData> settings, ILog log, IMaintainSearchIndexes<FrameworkMetaData> searchIndexMaintainer)
        {
            _settings = settings;
            _log = log;
            _searchIndexMaintainer = searchIndexMaintainer;
        }

        public async Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<FrameworkMetaData> entries)
        {
            try
            {
                _log.Debug("Indexing " + entries.Count + " standards");

                var indexNameAndDateExtension = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);
                await _searchIndexMaintainer.IndexEntries(indexNameAndDateExtension, entries);

                // Index frameworks
            }
            catch (Exception ex)
            {
                _log.Error("Error indexing PDFs", ex);
            }
        }

        public Task<ICollection<FrameworkMetaData>> LoadEntries()
        {
            var list = GetList();
            return list;
        }

        private async Task<ICollection<FrameworkMetaData>> GetList()
        {
            var list = new List<FrameworkMetaData>();
            var fw = new FrameworkMetaData
            {
                FworkCode = "12345",
                IssuingAuthorityTitle = "Author Title",
                NASTitle = "NasTitle",
                PathwayName = "PWName",
                PwayCode = "1",
                ProgType = "Ptype"
            };
            list.Add(fw);

            return list;
        }

        public Task IndexEntries(string indexName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            var oneDayAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-1), _settings.IndexesAlias, "yyyy-MM-dd");
            var twoDaysAgo2 = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-2), _settings.IndexesAlias, "yyyy-MM-dd");

            return _searchIndexMaintainer.DeleteIndexes(x => x.StartsWith(oneDayAgo2) || x.StartsWith(twoDaysAgo2));
        }

        public bool IsIndexCorrectlyCreated(string indexName)
        {
            throw new NotImplementedException();
        }

        public bool CreateIndex(string indexName)
        {
            throw new NotImplementedException();
        }

        public void SwapIndexes(string newIndexName)
        {
            throw new NotImplementedException();
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            var indexName = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);

            return _searchIndexMaintainer.IndexContainsDocuments(indexName);
        }

        public bool CreateIndex(DateTime scheduledRefreshDateTime)
        {
            var indexName = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);
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

        public void SwapIndexes(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = _settings.IndexesAlias;
            var newIndexName = IndexerHelper.GetIndexNameAndDateExtension(scheduledRefreshDateTime, _settings.IndexesAlias);

            if (!_searchIndexMaintainer.AliasExists(indexAlias))
            {
                _log.Warn("Alias doesn't exists, creating a new one...");

                _searchIndexMaintainer.CreateIndexAlias(_settings.IndexesAlias, newIndexName);
            }

            _searchIndexMaintainer.SwapAliasIndex(indexAlias, newIndexName);
        }
    }
}
