namespace Sfa.Infrastructure.Elasticsearch
{
    using System;

    using Nest;

    using Sfa.Infrastructure.Services;
    using Sfa.Infrastructure.Settings;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IInfrastructureSettings _infrastructureSettings;

        private readonly ConnectionSettings _connectionSettings;

        public ElasticsearchClientFactory(IInfrastructureSettings infrastructureSettings)
        {
            _infrastructureSettings = infrastructureSettings;
            _connectionSettings = new ConnectionSettings(new Uri(_infrastructureSettings.SearchHost));
        }

        public IElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }
    }
}