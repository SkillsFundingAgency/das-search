namespace Sfa.Infrastructure.Elasticsearch
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}