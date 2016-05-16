using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Nest;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    using Sfa.Das.Sas.ApplicationServices;

    public class ElasticsearchCustomClient : IElasticsearchCustomClient
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly ILog _logger;

        private readonly IProfileAStep _profiler;

        public ElasticsearchCustomClient(IElasticsearchClientFactory elasticsearchClientFactory, ILog logger, IProfileAStep profiler)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
            _profiler = profiler;
        }

        public ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector, [CallerMemberName] string callerName = "")
            where T : class
        {
            var client = _elasticsearchClientFactory.Create();
            using (_profiler.CreateStep($"Elasticsearch.Search.{callerName}"))
            {
                var result = client.Search(selector);

                SendLog(result, $"Elasticsearch.Search.{callerName}");
                return result;
            }
        }

        private void SendLog<T>(ISearchResponse<T> result, string identifier)
            where T : class
        {
            string body = string.Empty;
            if (result.ApiCall.RequestBodyInBytes != null)
            {
                body = System.Text.Encoding.Default.GetString(result.ApiCall.RequestBodyInBytes);
            }

            var properties = new Dictionary<string, object>
                                 {
                                     { "Identifier", identifier },
                                     {
                                         "HttpStatusCode",
                                         result.ApiCall?.HttpStatusCode
                                     },
                                     { "ResponseTime", result.Took },
                                     { "Uri", result.ApiCall?.Uri?.AbsoluteUri },
                                     { "RequestBody", body }
                                 };

            _logger.Debug($"ElasticsearchQuery: {identifier}", properties);
        }
    }
}
