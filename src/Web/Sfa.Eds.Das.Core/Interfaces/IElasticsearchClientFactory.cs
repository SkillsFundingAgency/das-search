namespace Sfa.Eds.Das.Core.Interfaces
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        ElasticClient Create();
    }
}