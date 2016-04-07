namespace Sfa.Eds.Das.Infrastructure.Elasticsearch
{
    using Nest;

    using System;
    using System.Collections.Generic;

    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Infrastructure.ElasticSearch;

    public class ElasticsearchCustomClient : IElasticsearchCustomClient
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly ILog _logger;

        public ElasticsearchCustomClient(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
        }

        public ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : class
        {
            var client = _elasticsearchClientFactory.Create();
            var result = client.Search(selector);
            SendLog(result, "Elasticsearch.Search");
            return result;
        }

        private void SendLog<T>(ISearchResponse<T> result, string identifier)
            where T : class
        {
            string body = string.Empty;
            if (result.ApiCall.RequestBodyInBytes != null)
            {
                body = System.Text.Encoding.Default.GetString(result.ApiCall.RequestBodyInBytes);
            }

            var properties = new Dictionary<string, object>()
                                 {
                                     { "Identifier", identifier },
                                     {
                                         "HttpStatusCode",
                                         result.ApiCall?.HttpStatusCode?.ToString()
                                     },
                                     { "ResponseTime", result.Took.ToString() },
                                     { "Uri", result.ApiCall?.Uri?.AbsoluteUri },
                                     { "RequestBody", body }
                                 };

            _logger.Info(identifier, properties);
        }
    }
}
