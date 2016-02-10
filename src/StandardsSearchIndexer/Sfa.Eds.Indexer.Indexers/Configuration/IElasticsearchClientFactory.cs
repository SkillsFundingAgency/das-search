using Nest;

namespace Sfa.Eds.Indexer.Indexer.Infrastructure.Configuration
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}