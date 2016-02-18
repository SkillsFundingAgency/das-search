using Nest;
using Sfa.Eds.Indexer.Common.AzureAbstractions;
using Sfa.Eds.Indexer.Common.Configuration;
using Sfa.Eds.Indexer.Common.Helpers;
using Sfa.Eds.Indexer.DedsService.Services;
using Sfa.Eds.Indexer.Settings.Settings;
using Sfa.Eds.Indexer.StandardIndexer.Consumers;
using Sfa.Eds.Indexer.StandardIndexer.Helpers;
using Sfa.Eds.Indexer.StandardIndexer.Services;
using Sfa.Eds.ProviderIndexer.Consumers;
using Sfa.Eds.ProviderIndexer.Helpers;
using Sfa.Eds.ProviderIndexer.Services;
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
            For<ILog>().AlwaysUnique().Use(x => LogManager.GetLogger(x.ParentType) ?? LogManager.GetLogger(x.RootType));
        }
    }
}
