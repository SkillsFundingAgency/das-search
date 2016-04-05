using Elasticsearch.Net;
using Nest;
using Sfa.Infrastructure.Settings;

namespace Sfa.Infrastructure.Elasticsearch
{
    using System;
    public class ElasticsearchClientFactory : IElasticsearchClientFactory, IDisposable
    {
        private ConnectionSettings _connectionSettings;

        public ElasticsearchClientFactory(IInfrastructureSettings infrastructureSettings)
        {
            _connectionSettings = new ConnectionSettings(new StaticConnectionPool(infrastructureSettings.ElasticServerUrls));
        }

        public IElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }

        public void Dispose()
        {
            _connectionSettings = null;
        }
    }
}