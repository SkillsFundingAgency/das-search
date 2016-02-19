using log4net;
using Nest;
using Sfa.DedsService.Services;
using Sfa.Eds.Das.Indexer.Common.AzureAbstractions;
using Sfa.Eds.Das.Indexer.Common.Configuration;
using Sfa.Eds.Das.Indexer.Common.Helpers;
using Sfa.Eds.Das.ProviderIndexer.Consumers;
using Sfa.Eds.Das.ProviderIndexer.Helpers;
using Sfa.Eds.Das.ProviderIndexer.Services;
using Sfa.Eds.Das.StandardIndexer.Consumers;
using Sfa.Eds.Das.StandardIndexer.Helpers;
using Sfa.Eds.Das.StandardIndexer.Services;
using Sfa.Eds.Indexer.Settings.Settings;
using StructureMap;

namespace Sfa.Eds.Indexer.AzureWorkerRole.DependencyResolution
{
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
            For<IDedsService>().Use<Sfa.DedsService.Services.DedsService>();
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
