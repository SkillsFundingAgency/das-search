using System;
using Nest;
using Sfa.Eds.Das.Indexer.Common.Settings;

namespace Sfa.Eds.Das.Indexer.Common.Configuration
{
    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ConnectionSettings _connectionSettings;
        private readonly ICommonSettings _commonSettings;

        public ElasticsearchClientFactory(ICommonSettings commonSettings)
        {
            _commonSettings = commonSettings;
            _connectionSettings = new ConnectionSettings(new Uri(_commonSettings.SearchHost));
        }

        public IElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }
    }
}