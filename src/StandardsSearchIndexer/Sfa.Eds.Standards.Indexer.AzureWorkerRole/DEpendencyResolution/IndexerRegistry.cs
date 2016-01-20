using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using StructureMap;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.DEpendencyResolution
{
    using log4net;

    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IStandardService>().Use<StandardService>();
            For<IStandardControlQueueConsumer>().Use<StandardControlQueueConsumer>();
            For<ILog>().AlwaysUnique().Use(x => x.ParentType == null
                    ? LogManager.GetLogger(x.RootType)
                    : LogManager.GetLogger(x.ParentType));
        }
    }
}