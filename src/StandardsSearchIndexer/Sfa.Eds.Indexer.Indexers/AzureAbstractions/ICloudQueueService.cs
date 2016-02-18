namespace Sfa.Eds.Indexer.Common.AzureAbstractions
{
    public interface ICloudQueueService
    {
        ICloudQueueWrapper GetQueueReference(string connectionString, string queueName);
    }
}