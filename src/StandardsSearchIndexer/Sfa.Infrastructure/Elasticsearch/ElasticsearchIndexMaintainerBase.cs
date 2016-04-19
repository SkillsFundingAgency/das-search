namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nest;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public abstract class ElasticsearchIndexMaintainerBase : IMaintainSearchIndexes
    {
        private readonly string _typeOfIndex;

        protected ElasticsearchIndexMaintainerBase(IElasticsearchCustomClient elasticsearchCustomClient, IElasticsearchMapper elasticsearchMapper, ILog log, string typeOfIndex)
        {
            Client = elasticsearchCustomClient;
            Log = log;
            ElasticsearchMapper = elasticsearchMapper;
            _typeOfIndex = typeOfIndex;
        }

        protected IElasticsearchCustomClient Client { get; }

        protected ILog Log { get; }

        protected IElasticsearchMapper ElasticsearchMapper { get; }

        public virtual bool AliasExists(string aliasName)
        {
            var aliasExistsResponse = Client.AliasExists(a => a.Name(aliasName));

            return aliasExistsResponse.Exists;
        }

        public abstract void CreateIndex(string indexName);

        public virtual void CreateIndexAlias(string aliasName, string indexName)
        {
            Client.Alias(a => a.Add(add => add.Index(indexName).Alias(aliasName)));
        }

        public virtual bool DeleteIndex(string indexName)
        {
            return Client.DeleteIndex(indexName).Acknowledged;
        }

        public virtual bool DeleteIndexes(Func<string, bool> indexNameMatch)
        {
            var result = true;

            var indicesToBeDelete = Client.IndicesStats(Indices.All).Indices.Select(x => x.Key).Where(indexNameMatch);

            Log.Debug($"Deleting {indicesToBeDelete.Count()} old {_typeOfIndex} indexes...");

            foreach (var index in indicesToBeDelete)
            {
                Log.Debug($"Deleting {index}");
                result = result && this.DeleteIndex(index);
            }

            Log.Debug("Deletion completed...");

            return result;
        }

        public virtual bool IndexContainsDocuments(string indexName)
        {
            var a = Client.Search<dynamic>(s => s.Index(indexName).AllTypes().From(0).Size(10).MatchAll()).Documents;

            return a.Any();
        }

        public virtual bool IndexExists(string indexName)
        {
            return Client.IndexExists(indexName).Exists;
        }

        public virtual void SwapAliasIndex(string aliasName, string newIndexName)
        {
            var existingIndexesOnAlias = Client.GetIndicesPointingToAlias(aliasName);
            var aliasRequest = new BulkAliasRequest { Actions = new List<IAliasAction>() };

            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = aliasName, Index = existingIndexOnAlias } });
            }

            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = aliasName, Index = newIndexName } });

            Client.Alias(aliasRequest);
        }

        protected static BulkDescriptor CreateBulkDescriptor(string indexName)
        {
            var bulkDescriptor = new BulkDescriptor();
            bulkDescriptor.Index(indexName);

            return bulkDescriptor;
        }

        protected static bool HaveReachedBatchLimit(int count)
        {
            const int batchSize = 4000;

            return count >= batchSize;
        }

        protected void LogResponse(IBulkResponse[] elementIndexResult)
        {
            foreach (var bulkResponse in elementIndexResult.Where(bulkResponse => bulkResponse.Errors))
            {
                ReportErrors(bulkResponse);
            }
        }

        private void ReportErrors(IBulkResponse result)
        {
            foreach (var message in result.ItemsWithErrors.Select(itemsWithError => string.Concat("Error indexing entry ", itemsWithError.Id, " at ", itemsWithError.Index)))
            {
                Log.Warn(message);
            }
        }
    }
}