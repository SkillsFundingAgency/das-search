namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        IElasticClient Create();
    }
}