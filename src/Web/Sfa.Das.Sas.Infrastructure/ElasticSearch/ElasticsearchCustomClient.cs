using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Nest;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Logging;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public class ElasticsearchCustomClient : IElasticsearchCustomClient
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly ILog _logger;
        private readonly IProfileAStep _profiler;

        public ElasticsearchCustomClient(
            IElasticsearchClientFactory elasticsearchClientFactory, 
            ILog logger, 
            IProfileAStep profiler)
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
                var stopwatch = Stopwatch.StartNew();
                var result = client.Search(selector);

                SendLog(result, $"Elasticsearch.Search.{callerName}", stopwatch.Elapsed);
                return result;
            }
        }

        private void SendLog<T>(ISearchResponse<T> result, string identifier, TimeSpan clientRequestTime)
            where T : class
        {
            string body = string.Empty;
            if (result.ApiCall.RequestBodyInBytes != null)
            {
                body = System.Text.Encoding.Default.GetString(result.ApiCall.RequestBodyInBytes);
            }
            
            var logEntry = new ElasticSearchLogEntry
                {
                    Identifier = identifier,
                    ReturnCode = result.ApiCall?.HttpStatusCode,
                    Successful = result.ApiCall?.Success,
                    SearchTime = result.Took,
                    RequestTime = Math.Round(clientRequestTime.TotalMilliseconds, 2),
                    MaxScore = result.MaxScore,
                    HitCount = result.Hits?.Count(),
                    Url = result.ApiCall?.Uri?.AbsoluteUri,
                    Body = body
                };
            
            _logger.Debug("Elastic Search Requested", logEntry);
        }
    }
}
