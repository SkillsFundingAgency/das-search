namespace Sfa.Eds.Indexer.Indexer.Infrastructure.AzureAbstractions
{
    public interface ICloudQueueService
    {
        ICloudQueueWrapper GetQueueReference(string connectionString, string queueName);
    }
}