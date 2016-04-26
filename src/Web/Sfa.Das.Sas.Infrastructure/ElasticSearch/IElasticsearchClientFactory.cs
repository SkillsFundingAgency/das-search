using Nest;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient Create();
    }
}