namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using global::Elasticsearch.Net;

    using Nest;

    using Sfa.Eds.Das.Indexer.Core.Services;

    public class ElasticsearchCustomClient : IElasticsearchCustomClient
    {
        private readonly ILog _logger;

        private readonly IElasticClient _client;

        public ElasticsearchCustomClient(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger)
        {
            _client = elasticsearchClientFactory.GetElasticClient();
            _logger = logger;
        }

        public ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector, [CallerMemberName] string callerName = "")
            where T : class
        {
            var result = _client.Search(selector);
            SendLog(result.ApiCall, result.Took, $"Elasticsearch.Search.{callerName}");
            return result;
        }

        public IExistsResponse IndexExists(IndexName index, [CallerMemberName] string callerName = "")
        {
            var result = _client.IndexExists(index);
            SendLog(result.ApiCall, null, $"Elasticsearch.IndexExists.{callerName}");
            return result;
        }

        public IDeleteIndexResponse DeleteIndex(IndexName index, [CallerMemberName] string callerName = "")
        {
            var result = _client.DeleteIndex(index);
            SendLog(result.ApiCall, null, $"Elasticsearch.DeleteIndex.{callerName}");
            return result;
        }

        public IGetMappingResponse GetMapping<T>(Func<GetMappingDescriptor<T>, IGetMappingRequest> selector = null, [CallerMemberName] string callerName = "")
            where T : class
        {
            var result = _client.GetMapping(selector);
            SendLog(result.ApiCall, null, $"Elasticsearch.GetMapping.{callerName}");
            return result;
        }

        public IRefreshResponse Refresh(IRefreshRequest request, [CallerMemberName] string callerName = "")
        {
            var result = _client.Refresh(request);
            SendLog(result.ApiCall, null, $"Elasticsearch.Refresh.{callerName}");
            return result;
        }

        public IRefreshResponse Refresh(Indices indices, Func<RefreshDescriptor, IRefreshRequest> selector = null, string callerName = "")
        {
            var result = _client.Refresh(indices);
            SendLog(result.ApiCall, null, $"Elasticsearch.Refresh.{callerName}");
            return result;
        }

        public IExistsResponse AliasExists(Func<AliasExistsDescriptor, IAliasExistsRequest> selector, string callerName = "")
        {
            var result = _client.AliasExists(selector);
            SendLog(result.ApiCall, null, $"Elasticsearch.AliasExists.{callerName}");
            return result;
        }

        public IBulkAliasResponse Alias(Func<BulkAliasDescriptor, IBulkAliasRequest> selector, string callerName = "")
        {
            var result = _client.Alias(selector);
            SendLog(result.ApiCall, null, $"Elasticsearch.Alias.{callerName}");
            return result;
        }

        public IBulkAliasResponse Alias(IBulkAliasRequest request, string callerName = "")
        {
            var result = _client.Alias(request);
            SendLog(result.ApiCall, null, $"Elasticsearch.Alias.{callerName}");
            return result;
        }

        public IIndicesStatsResponse IndicesStats(Indices indices, Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null, string callerName = "")
        {
            var result = _client.IndicesStats(indices, selector);
            SendLog(result.ApiCall, null, $"Elasticsearch.IndicesStats.{callerName}");
            return result;
        }

        public IList<string> GetIndicesPointingToAlias(string aliasName, string callerName = "")
        {
            var result = _client.GetIndicesPointingToAlias(aliasName);
            SendLog(null, null, $"Elasticsearch.GetIndicesPointingToAlias.{callerName}");
            return result;
        }

        public ICreateIndexResponse CreateIndex(IndexName index, Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null, string callerName = "")
        {
            var result = _client.CreateIndex(index, selector);
            SendLog(result.ApiCall, null, $"Elasticsearch.CreateIndex(.{callerName}");
            return result;
        }

        public Task<IBulkResponse> BulkAsync(IBulkRequest request, string callerName = "")
        {
            var result = _client.BulkAsync(request);
            SendLog(null, null, $"Elasticsearch.BulkAsync.{callerName}");
            return result;
        }

        public Task<IIndexResponse> IndexAsync<T>(T @object, Func<IndexDescriptor<T>, IIndexRequest> selector = null, string callerName = "")
            where T : class
        {
            var result = _client.IndexAsync(@object, selector);
            SendLog(null, null, $"Elasticsearch.IndexAsync.{callerName}");
            return result;
        }

        private void SendLog(IApiCallDetails apiCallDetails, long? took, string identifier)
        {
            string body = string.Empty;
            if (apiCallDetails?.RequestBodyInBytes != null)
            {
                body = System.Text.Encoding.Default.GetString(apiCallDetails.RequestBodyInBytes);
            }

            var properties = new Dictionary<string, object>()
                                 {
                                     { "Identifier", identifier },
                                     {
                                         "HttpStatusCode",
                                         apiCallDetails?.HttpStatusCode
                                     },
                                     { "ResponseTime", took },
                                     { "Uri", apiCallDetails?.Uri?.AbsoluteUri },
                                     { "RequestBody", body }
                                 };
            _logger.Info(identifier, properties);
        }
    }
}