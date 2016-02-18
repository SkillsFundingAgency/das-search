using Nest;

namespace Sfa.Eds.Indexer.Common.Configuration
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}