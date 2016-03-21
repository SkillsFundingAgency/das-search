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
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IWorkerRoleSettings>().Use<WorkRoleSettings>();
            For<IBlobStorageHelper>().Use<BlobStorageHelper>();
            For<IGenericControlQueueConsumer>().Use<GenericControlQueueConsumer>();
            For<IGetMessageTimes>().Use<AzureQueueService>();
            For<IClearQueue>().Use<AzureQueueService>();
        }
    }
}