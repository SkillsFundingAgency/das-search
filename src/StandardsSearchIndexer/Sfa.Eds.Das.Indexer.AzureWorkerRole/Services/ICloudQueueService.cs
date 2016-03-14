namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using Microsoft.WindowsAzure.Storage.Queue;

    public interface ICloudQueueService
    {
        CloudQueue GetQueueReference(string connectionString, string queueName);
    }
}