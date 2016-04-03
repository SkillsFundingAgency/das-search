using Elasticsearch.Net;
using Nest;
using Sfa.Infrastructure.Settings;

namespace Sfa.Infrastructure.Elasticsearch
{
    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ConnectionSettings _connectionSettings;

        public ElasticsearchClientFactory(IInfrastructureSettings infrastructureSettings)
        {
            var pool = new StaticConnectionPool(infrastructureSettings.ElasticServerUrls);
            _connectionSettings = new ConnectionSettings(pool);
        }

        public IElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }
    }
}