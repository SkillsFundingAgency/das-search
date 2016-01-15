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
        //private string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=indexerstorage;AccountKey=jVvVtb02mUNn/QiFJB71czOBNXqMYxj0UIpq3paGO+u3MiXzqxrbz7rO9RYASYkD/JXiRlWxn/s/o/lFYujkaA==";

        public CloudQueue GetQueue(string connectionstring, string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionstring);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(queueName);

            queue.CreateIfNotExists();
            return queue;
        }

        public void CheckMessage(string queueName)
        {
            Trace.TraceInformation("Entramos al metodo de mensajes");

            var queue = GetQueue(_connectionString, queueName);
            Trace.TraceInformation("Reading the queue");
            var messages = queue.GetMessages(10).OrderByDescending(x => x.InsertionTime);
            Trace.TraceInformation("Getting messages");
            if (messages.Any())
            {
                Trace.TraceInformation("There is at least one message");
                var message = messages.FirstOrDefault();
                if (message != null)
                {
                    try
                    {
                        var standardService = new StandardService();
                        Trace.TraceInformation("Going to standard service");
                        standardService.CreateScheduledIndex(message.InsertionTime.Value.DateTime);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.Message);
                        var error = e;
                        throw;
                    }

                }
            }

            Trace.TraceInformation("Delete messages");

            foreach (var cloudQueueMessage in messages)
            {
                queue.DeleteMessage(cloudQueueMessage);
            }

            var testDeletion = queue.GetMessages(10);
        }
    }
}