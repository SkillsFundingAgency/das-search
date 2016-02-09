namespace Sfa.Eds.Indexer.Indexers.AzureAbstractions
{
    public interface ICloudQueueService
    {
        ICloudQueueWrapper GetQueueReference(string connectionString, string queueName);
    }
}