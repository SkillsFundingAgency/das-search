using Sfa.Das.Sas.Indexer.ApplicationServices.Queue;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.Settings;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole.DependencyResolution
{
    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IIndexerJob>().Use<IndexerJob>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IWorkerRoleSettings>().Use<WorkRoleSettings>();
            For<IGenericControlQueueConsumer>().Use<GenericControlQueueConsumer>();
            For<IProvideSettings>().Use(c => new AppConfigSettingsProvider(new MachineSettings()));
        }
    }
}