namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution
{
    using Nest;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Services;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;
    using Sfa.Infrastructure.Elasticsearch;
    using Sfa.Infrastructure.Services;

    using StructureMap;

    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IIndexerJob>().Use<IndexerJob>();
            For<IElasticClient>().Use<ElasticClient>(); // TODO: LWA - We shouldn't have a referenece to nest. Remove Nest package.
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IWorkerRoleSettings>().Use<WorkRoleSettings>();
            For<IGenericControlQueueConsumer>().Use<GenericControlQueueConsumer>();
            For<IGetMessageTimes>().Use<AzureQueueService>();
            For<IClearQueue>().Use<AzureQueueService>();
        }
    }
}