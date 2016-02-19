using log4net;
using Nest;
using Sfa.Eds.Das.Indexer.Common.AzureAbstractions;
using Sfa.Eds.Das.Indexer.Common.Configuration;
using Sfa.Eds.Das.Indexer.Common.Helpers;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution
{
    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IIndexerScheduler>().Use<IndexerScheduler>();
            For<ILog>().AlwaysUnique().Use(x => LogManager.GetLogger(x.ParentType) ?? LogManager.GetLogger(x.RootType));
        }
    }
}
