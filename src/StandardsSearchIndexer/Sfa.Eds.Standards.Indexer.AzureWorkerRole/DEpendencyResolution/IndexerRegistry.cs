using Nest;
using Sfa.Eds.Indexer.DedsService.Services;
using Sfa.Eds.Indexer.Indexers.AzureAbstractions;
using Sfa.Eds.Indexer.Indexers.Configuration;
using Sfa.Eds.Indexer.Indexers.Consumers;
using Sfa.Eds.Indexer.Indexers.Helpers;
using Sfa.Eds.Indexer.Indexers.Services;
using Sfa.Eds.Indexer.Settings.Settings;
using StructureMap;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.DependencyResolution
{
    using log4net;

    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IStandardIndexerService>().Use<StandardIndexerService>();
            For<IProviderIndexerService>().Use<ProviderIndexerService>();
            For<IStandardHelper>().Use<StandardHelper>();
            For<IProviderHelper>().Use<ProviderHelper>();
            For<IStandardControlQueueConsumer>().Use<StandardControlQueueConsumer>();
            For<IProviderControlQueueConsumer>().Use<ProviderControlQueueConsumer>();
            For<IDedsService>().Use<Eds.Indexer.DedsService.Services.DedsService>();
            For<IStandardIndexSettings>().Use<StandardIndexSettings>();
            For<IProviderIndexSettings>().Use<ProviderIndexSettings>();
            For<IBlobStorageHelper>().Use<BlobStorageHelper>();
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IIndexerScheduler>().Use<IndexerScheduler>();
            For<ICloudQueueService>().Use<CloudQueueService>();
            For<ILog>().AlwaysUnique().Use(x => x.ParentType == null
                    ? LogManager.GetLogger(x.RootType)
                    : LogManager.GetLogger(x.ParentType));
        }
    }
}
