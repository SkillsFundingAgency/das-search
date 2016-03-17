namespace Sfa.Infrastructure.Services
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        IElasticClient GetElasticClient();
    }
}