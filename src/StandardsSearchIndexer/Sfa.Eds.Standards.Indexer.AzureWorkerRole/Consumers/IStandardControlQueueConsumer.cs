using System.Threading.Tasks;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public interface IStandardControlQueueConsumer
    {
        Task CheckMessage();
    }
}