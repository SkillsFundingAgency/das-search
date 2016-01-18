using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public class StandardControlQueueConsumer
    {
        private static readonly StandardIndexSettings StandardIndexSettings = new StandardIndexSettings();
        private readonly string _connectionString = StandardIndexSettings.ConnectionString;

        public static CloudQueue GetQueue(string connectionstring, string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionstring);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(queueName);

            queue.CreateIfNotExists();
            return queue;
        }

        public void CheckMessage(string queueName)
        {
            var queue = GetQueue(_connectionString, queueName);
            var messages = queue.GetMessages(10).OrderByDescending(x => x.InsertionTime);

            if (messages.Any())
            {
                var message = messages.FirstOrDefault();
                if (message != null)
                {
                    try
                    {
                        var standardService = new StandardService();
                        StandardService.CreateScheduledIndex(message.InsertionTime.Value.DateTime);
                    }
                    catch (Exception e)
                    {
                        var error = e;
                        throw;
                    }
                }
            }

            foreach (var cloudQueueMessage in messages)
            {
                queue.DeleteMessage(cloudQueueMessage);
            }
        }
    }
}