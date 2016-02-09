using Nest;

namespace Sfa.Eds.Indexer.Indexers.Configuration
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}