using System;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;

namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Models
{
    public class AzureQueueMessage : IQueueMessage
    {
        public AzureQueueMessage(CloudQueueMessage message)
        {
            AzureMessage = message;
        }

        /// <summary>
        /// The azure queue message that has come from he azure queue itself
        /// </summary>
        public CloudQueueMessage AzureMessage { get; }

        /// <summary>
        /// Gets message of the queue message
        /// </summary>
        public string Message => AzureMessage.AsString;

        /// <summary>
        /// Gets the time the message was added to the message queue
        /// </summary>
        public DateTime? InsertionTime => AzureMessage?.InsertionTime?.DateTime;
    }
}
