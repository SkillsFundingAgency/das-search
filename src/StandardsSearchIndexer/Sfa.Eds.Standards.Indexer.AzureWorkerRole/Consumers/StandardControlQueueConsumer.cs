using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public class StandardControlQueueConsumer
    {
        private readonly string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];

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
                        standardService.CreateScheduledIndex(message.InsertionTime.Value.DateTime);
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