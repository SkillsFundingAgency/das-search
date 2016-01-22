namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.AzureAbstractions
{
    public interface ICloudQueueService
    {
        ICloudQueueWrapper GetQueueReference(string connectionString, string queueName);
    }
}