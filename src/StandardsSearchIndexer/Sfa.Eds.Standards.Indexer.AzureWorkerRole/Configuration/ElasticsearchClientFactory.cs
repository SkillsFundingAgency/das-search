using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration
{
    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IStandardIndexSettings _standardIndexSettings;
        private readonly ConnectionSettings _connectionSettings;

        public ElasticsearchClientFactory(IStandardIndexSettings standardIndexSettings)
        {
            _standardIndexSettings = standardIndexSettings;
            _connectionSettings = new ConnectionSettings(new Uri(_standardIndexSettings.SearchHost));
        }

        public IElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        }
    }
}
