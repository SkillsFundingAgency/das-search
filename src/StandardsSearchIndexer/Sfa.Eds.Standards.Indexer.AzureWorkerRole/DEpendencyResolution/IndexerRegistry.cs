using log4net;
using Nest;
using Sfa.Eds.Das.Indexer.Common.Configuration;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution
{
    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IIndexerJob>().Use<IndexerJob>();
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<ILog>().AlwaysUnique().Use(x => LogManager.GetLogger(x.ParentType) ?? LogManager.GetLogger(x.RootType));
        }
    }
}
