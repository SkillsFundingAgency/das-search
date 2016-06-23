namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration
{
    using Nest;

    public interface IElasticsearchConfiguration
    {
        AnalysisDescriptor ApprenticeshipAnalysisDescriptor();
    }
}