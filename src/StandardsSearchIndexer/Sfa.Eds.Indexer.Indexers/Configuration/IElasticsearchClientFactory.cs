using Nest;

namespace Sfa.Eds.Das.Indexer.Common.Configuration
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}