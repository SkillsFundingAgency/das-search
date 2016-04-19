namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Models;
    using Sfa.Eds.Das.Indexer.Core.Services;

    public class AzureCloudQueueService : IMessageQueueService
    {
        private readonly IAppServiceSettings _appServiceSettings;
        private readonly ILog _logger;

        public AzureCloudQueueService(IAppServiceSettings appServiceSettings, ILog logger)
        {
            _appServiceSettings = appServiceSettings;
            _logger = logger;
        }

        public int DefaultMessageCount { get; set; } = 100;

        /// <summary>
        /// Gets a default number of messages for the given queue
        /// </summary>
        /// <param name="queueName">name fo the queue you want messages from</param>
        /// <returns>A default number of messages from the named queue</returns>
        public IEnumerable<IQueueMessage> GetQueueMessages(string queueName)
        {
            _logger.Debug("Getting " + DefaultMessageCount + " messages from queue [" + queueName + "]");
            var queue = GetQueueReference(queueName);

            return queue?.GetMessages(DefaultMessageCount).Select(x => new AzureQueueMessage(x));
        }

        /// <summary>
        /// Gets the number of messages on the current named queue
        /// </summary>
        /// <param name="queueName">Name of the queue to get the message count for</param>
        /// <returns>Number of messages currently on that queue</returns>
        public int GetQueueMessageCount(string queueName)
        {
            _logger.Debug("Getting messages count from queue [" + queueName + "]");
            var queue = GetQueueReference(queueName);
            queue?.FetchAttributes();

            return queue?.ApproximateMessageCount ?? 0;
        }

        /// <summary>
        /// Get messages from a queue
        /// </summary>
        /// <param name="queueName">Queue name to get messages from</param>
        /// <param name="messageCount">The maximum number of messages to collect.</param>
        /// <returns>[message count] number of messages or all messages (whichever is less) from the queue</returns>
        public IEnumerable<IQueueMessage> GetQueueMessages(string queueName, int messageCount)
        {
            _logger.Debug("Getting " + messageCount + " messages from queue [" + queueName + "]");
            var queue = GetQueueReference(queueName);
            return queue?.GetMessages(messageCount, TimeSpan.FromSeconds(30)).Select(x => new AzureQueueMessage(x));
        }

        /// <summary>
        /// Deletes a single message from the given queue
        /// </summary>
        /// <param name="queueName">Queue name to delete the message from</param>
        /// <param name="message">The message to delete off the queue</param>
        public void DeleteQueueMessage(string queueName, IQueueMessage message)
        {
            var azureQueueMessage = message as AzureQueueMessage;

            if (azureQueueMessage == null)
            {
                return;
            }

            var queue = GetQueueReference(queueName);
            queue.DeleteMessage(azureQueueMessage.AzureMessage);
        }

        /// <summary>
        /// Deletes a collection of messages from the given queue
        /// </summary>
        /// <param name="queueName">Name of the queue you want to delete the messages from</param>
        /// <param name="messages">Collection of messages to delete from the named queue</param>
        public void DeleteQueueMessages(string queueName, IEnumerable<IQueueMessage> messages)
        {
            _logger.Debug("Deleteing messages from queue [" + queueName + "]");

            var queue = GetQueueReference(queueName);

            if (queue == null)
            {
                _logger.Debug("No queue with that name found");
                return;
            }

            var azureCloudMessages = messages.OfType<AzureQueueMessage>();

            foreach (var message in azureCloudMessages)
            {
                queue.DeleteMessage(message.AzureMessage);
            }
        }

        /// <summary>
        /// Clear all messages from the given queue
        /// </summary>
        /// <param name="queueName">Name of the queue you want to delete all messages from</param>
        public void ClearQueue(string queueName)
        {
            _logger.Debug("Clearing messages from queue [" + queueName + "]");
            var queue = GetQueueReference(queueName);
            queue?.Clear();
        }

        private CloudQueue GetQueueReference(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(_appServiceSettings.ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            return queue;
        }
    }
}