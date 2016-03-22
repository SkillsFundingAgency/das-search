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

    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public sealed class StandardIndexer : IGenericIndexerHelper<MetaDataItem>
        // ToDo: Rename to ApprenticeshipIndexer
    {
        private readonly IIndexSettings<MetaDataItem> _settings;
        private readonly IMaintanStandardIndex _searchIndexMaintainer;
        private readonly IMetaDataHelper _metaDataHelper;
        private readonly ILog _log;

        public StandardIndexer(
            IIndexSettings<MetaDataItem> settings,
            IMaintanStandardIndex searchIndexMaintainer,
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

        public void SwapIndexes(string newIndexName)
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

                await _searchIndexMaintainer.IndexStandards(indexName, entries);
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

                await _searchIndexMaintainer.IndexFrameworks(indexName , entries);
            }
            catch (Exception ex)
            {
                _log.Error("Error indexing Frameworks", ex);
            }
        }

        private Task<ICollection<MetaDataItem>> LoadStandardMetaData()
        {
            _metaDataHelper.UpdateMetadataRepository();
            _log.Info("Indexing standard PDFs...");

            var standardsMetaData = _metaDataHelper.GetAllStandardsMetaData();
            return Task.FromResult<ICollection<MetaDataItem>>(standardsMetaData.ToList());
        }

        private string GenerateFrameworkMapping()
        {
            var mapping = new
            {
                properties = new
                {
                    frameworkCode = new
                    {
                        type = "string",
                    },
                    frameworkName = new
                    {
                        type = "string"
                    },
                    pathwayCode = new
                    {
                        type = "string"
                    },
                    pathwayName = new
                    {
                        type = "string"
                    },
                    title = new
                    {
                        type = "string"
                    }
                }
              };
            return ToJson(mapping);
        }

        private static readonly Func<object, string> ToJson = d => Newtonsoft.Json.JsonConvert.SerializeObject(d);
    }
}