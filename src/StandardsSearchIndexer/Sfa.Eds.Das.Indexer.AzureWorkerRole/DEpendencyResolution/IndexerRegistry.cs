namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution
{
    using Core.Services;
    using Infrastructure.Settings;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Services;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;
    using Sfa.Infrastructure.Elasticsearch;

    using StructureMap;

    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IIndexerJob>().Use<IndexerJob>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IWorkerRoleSettings>().Use<WorkRoleSettings>();
            For<IGenericControlQueueConsumer>().Use<GenericControlQueueConsumer>();
            For<IMessageQueueService>().Use<AzureCloudQueueService>();
            For<IProvideSettings>().Use(c => new AppConfigSettingsProvider(new MachineSettings()));
        }
    }
}