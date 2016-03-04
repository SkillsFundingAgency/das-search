using Nest;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution
{
    using Sfa.Deds.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Services;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;
    using Sfa.Infrastructure.Services;

    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IIndexerJob>().Use<IndexerJob>();
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IWorkerRoleSettings>().Use<WorkRoleSettings>();
            For<IBlobStorageHelper>().Use<BlobStorageHelper>();
        }
    }
}
