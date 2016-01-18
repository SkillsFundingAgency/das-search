using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using StructureMap;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.DEpendencyResolution
{
    public class IndexerRegistry : Registry
    {
        public IndexerRegistry()
        {
            For<IStandardService>().Use<StandardService>();
            For<IStandardControlQueueConsumer>().Use<StandardControlQueueConsumer>();
        }
    }
}