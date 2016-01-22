using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.AzureAbstractions
{
    public class CloudQueueService : ICloudQueueService
    {
        private readonly ICloudQueueWrapper _cloudQueueWrapper;

        public CloudQueueService(ICloudQueueWrapper cloudQueueWrapper)
        {
            _cloudQueueWrapper = cloudQueueWrapper;
        }

        public ICloudQueueWrapper GetQueueReference(string connectionString, string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            var queue = new CloudQueueWrapper(queueClient.GetQueueReference(queueName));
            queue.CreateIfNotExists();
            return queue;
        }
    }
}
