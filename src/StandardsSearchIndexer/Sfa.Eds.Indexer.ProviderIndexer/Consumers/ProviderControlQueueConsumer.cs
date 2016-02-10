using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Sfa.Eds.Indexer.Indexer.Infrastructure.AzureAbstractions;
using Sfa.Eds.Indexer.ProviderIndexer.Services;
using Sfa.Eds.Indexer.Settings.Settings;

namespace Sfa.Eds.Indexer.ProviderIndexer.Consumers
{
    public class ProviderControlQueueConsumer : IProviderControlQueueConsumer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ICloudQueueService _cloudQueueService;

        private readonly IProviderIndexSettings _providerIndexSettings;
        private readonly IProviderIndexerService _providerIndexerService;

        public ProviderControlQueueConsumer(
            IProviderIndexerService providerIndexerService,
            IProviderIndexSettings providerIndexSettings,
            ICloudQueueService cloudQueueService)
        {
            _providerIndexSettings = providerIndexSettings;
            _cloudQueueService = cloudQueueService;
            _providerIndexerService = providerIndexerService;
        }

        public Task CheckMessage()
        {
            return Task.Run(() =>
            {
                var queue = _cloudQueueService.GetQueueReference(_providerIndexSettings.ConnectionString, _providerIndexSettings.QueueName);
                var cloudQueueMessages = queue.GetMessages(10);
                var messages = cloudQueueMessages.OrderByDescending(x => x.InsertionTime);

                if (messages.Any())
                {
                    var message = messages.FirstOrDefault();
                    if (message != null)
                    {
                        Log.Info("Creating new scheduled provider index at " + DateTime.Now);
                        _providerIndexerService.CreateScheduledIndex(message.InsertionTime?.DateTime ?? DateTime.Now);
                    }
                }

                foreach (var cloudQueueMessage in messages)
                {
                    queue.DeleteMessage(cloudQueueMessage);
                }
            });
        }
    }
}