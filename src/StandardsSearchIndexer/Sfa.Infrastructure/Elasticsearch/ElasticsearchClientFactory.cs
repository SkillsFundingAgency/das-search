namespace Sfa.Infrastructure.Elasticsearch
{
    using global::Elasticsearch.Net.ConnectionPool;

    using Nest;
    using Sfa.Infrastructure.Settings;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ConnectionSettings _connectionSettings;

        public ElasticsearchClientFactory(IInfrastructureSettings infrastructureSettings)
        {
            var pool = new SniffingConnectionPool(infrastructureSettings.ElasticServerUrls);
            _connectionSettings = new ConnectionSettings(pool);
        }

        public IElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }
    }
}