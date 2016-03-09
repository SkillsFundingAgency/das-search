namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}