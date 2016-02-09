using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Sfa.Eds.Indexer.Indexers.AzureAbstractions
{
    public class CloudQueueWrapper : ICloudQueueWrapper
    {
        private readonly CloudQueue _cloudQueue;

        public CloudQueueWrapper(CloudQueue cloudQueue)
        {
            _cloudQueue = cloudQueue;
        }

        public bool CreateIfNotExists()
        {
            return _cloudQueue.CreateIfNotExists();
        }

        public IEnumerable<CloudQueueMessage> GetMessages(int messageCount)
        {
            return _cloudQueue.GetMessages(messageCount);
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            _cloudQueue.DeleteMessage(message);
        }
    }
}