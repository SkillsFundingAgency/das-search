namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration
{
    using Nest;

    public interface IElasticsearchConfiguration
    {
        AnalysisDescriptor ApprenticeshipAnalysisDescriptor();

        int ApprenticeshipIndexShards();

        int ApprenticeshipIndexReplicas();

        int ProviderIndexShards();

        int ProviderIndexReplicas();
    }
}