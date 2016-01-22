using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.AzureAbstractions
{
    public interface ICloudQueueWrapper
    {
        bool CreateIfNotExists();
        void DeleteMessage(CloudQueueMessage message);
        IEnumerable<CloudQueueMessage> GetMessages(int messageCount);
    }
}