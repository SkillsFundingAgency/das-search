using System.Threading.Tasks;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public interface IProviderControlQueueConsumer
    {
        Task CheckMessage();
    }
}