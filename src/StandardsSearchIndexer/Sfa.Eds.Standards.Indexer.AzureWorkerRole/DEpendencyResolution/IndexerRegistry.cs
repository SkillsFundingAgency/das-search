using Nest;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;
using StructureMap;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.DependencyResolution
{
    using log4net;

    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IStandardService>().Use<StandardService>();
            For<IStandardControlQueueConsumer>().Use<StandardControlQueueConsumer>();
            For<IDedsService>().Use<Services.DedsService>();
            For<IStandardIndexSettings>().Use<StandardIndexSettings>();
            For<IBlobStorageHelper>().Use<BlobStorageHelper>();
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<ILog>().AlwaysUnique().Use(x => x.ParentType == null
                    ? LogManager.GetLogger(x.RootType)
                    : LogManager.GetLogger(x.ParentType));
        }
    }
}