using Nest;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}