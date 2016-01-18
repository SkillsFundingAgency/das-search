using Microsoft.WindowsAzure.Storage.Queue;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public interface IStandardControlQueueConsumer
    {
        void CheckMessage(string queueName);
    }
}