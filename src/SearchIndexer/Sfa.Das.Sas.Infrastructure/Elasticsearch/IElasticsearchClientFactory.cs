using Nest;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}