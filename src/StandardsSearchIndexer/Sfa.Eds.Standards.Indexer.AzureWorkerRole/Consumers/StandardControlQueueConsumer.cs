using System;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.AzureAbstractions;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers
{
    public class StandardControlQueueConsumer : IStandardControlQueueConsumer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IStandardIndexSettings _standardIndexSettings;
        private readonly ICloudQueueService _cloudQueueService;
        private readonly IStandardService _standardService;

        public StandardControlQueueConsumer(IStandardService standardService, IStandardIndexSettings standardIndexSettings, ICloudQueueService cloudQueueService)
        {
            _standardIndexSettings = standardIndexSettings;
            _cloudQueueService = cloudQueueService;
            _standardService = standardService;
        }

        public void CheckMessage()
        {
            var queue = _cloudQueueService.GetQueueReference(_standardIndexSettings.ConnectionString, _standardIndexSettings.QueueName);
            var cloudQueueMessages = queue.GetMessages(10);
            var messages = cloudQueueMessages.OrderByDescending(x => x.InsertionTime);

            if (messages.Any())
            {
                var message = messages.FirstOrDefault();
                if (message != null)
                {
                    Log.Info("Creating new scheduled index at " + DateTime.Now);
                    _standardService.CreateScheduledIndex(message.InsertionTime?.DateTime ?? DateTime.Now);
                }
            }

            foreach (var cloudQueueMessage in messages)
            {
                queue.DeleteMessage(cloudQueueMessage);
            }
        }
    }
}