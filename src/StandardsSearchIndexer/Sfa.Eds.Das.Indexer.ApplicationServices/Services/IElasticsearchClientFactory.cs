namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}