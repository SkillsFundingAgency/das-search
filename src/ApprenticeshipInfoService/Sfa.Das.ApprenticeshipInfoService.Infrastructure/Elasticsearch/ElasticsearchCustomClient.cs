namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Elasticsearch
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Nest;
    using Sfa.Das.ApprenticeshipInfoService.Core.Logging;
    using Sfa.Das.ApprenticeshipInfoService.Infrastructure.Logging;

    public class ElasticsearchCustomClient : IElasticsearchCustomClient
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly ILog _logger;

        public ElasticsearchCustomClient(
            IElasticsearchClientFactory elasticsearchClientFactory,
            ILog logger)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
        }

        public ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector, [CallerMemberName] string callerName = "")
            where T : class
        {
            var client = _elasticsearchClientFactory.Create();
            var stopwatch = Stopwatch.StartNew();
            var result = client.Search(selector);

            SendLog(result, $"Elasticsearch.Search.{callerName}", stopwatch.Elapsed);
            return result;
        }

        private void SendLog<T>(ISearchResponse<T> result, string identifier, TimeSpan clientRequestTime)
            where T : class
        {
            var body = string.Empty;
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

            var dependencyLogEntry = new DependencyLogEntry
            {
                Identifier = identifier,
                ResponseCode = result.ApiCall?.HttpStatusCode,
                ResponseTime = Math.Round(clientRequestTime.TotalMilliseconds, 2),
                Url = result.ApiCall?.Uri?.AbsoluteUri
            };

            _logger.Debug("Elastic Search Requested", logEntry);
            _logger.Debug("Dependency Elasticsearch", dependencyLogEntry);
        }
    }
}
