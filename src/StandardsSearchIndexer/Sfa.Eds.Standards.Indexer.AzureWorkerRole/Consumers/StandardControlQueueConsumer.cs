using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public class StandardControlQueueConsumer : IStandardControlQueueConsumer
    {
        private readonly IStandardIndexSettings _standardIndexSettings;
        private readonly IStandardService _standardService;

        public StandardControlQueueConsumer(IStandardService standardService, IStandardIndexSettings standardIndexSettings)
        {
            _standardIndexSettings = standardIndexSettings;
            _standardService = standardService;
        }

        public void CheckMessage(string queueName)
        {
            var queue = GetQueue(queueName);
            var messages = queue.GetMessages(10).OrderByDescending(x => x.InsertionTime);

            if (messages.Any())
            {
                var message = messages.FirstOrDefault();
                if (message != null)
                {
                    try
                    {
                        _standardService.CreateScheduledIndex(message.InsertionTime?.DateTime ?? DateTime.Now);
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

        private CloudQueue GetQueue(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(_standardIndexSettings.ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(queueName);

            queue.CreateIfNotExists();
            return queue;
        }
    }
}