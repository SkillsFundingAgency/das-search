using System.Linq;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Queue
{
    using System;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    public class GenericControlQueueConsumer : IGenericControlQueueConsumer
    {
        private readonly IAppServiceSettings _appServiceSettings;
        private readonly IMessageQueueService _cloudQueueService;
        private readonly IContainer _container;
        private readonly ILog _log;

        public GenericControlQueueConsumer(IAppServiceSettings appServiceSettings, IMessageQueueService cloudQueueService, IContainer container, ILog log)
        {
            _appServiceSettings = appServiceSettings;
            _cloudQueueService = cloudQueueService;
            _container = container;
            _log = log;
        }

        public async Task CheckMessage<T>()
            where T : IMaintainSearchIndexes
        {
            var indexerService = _container.GetInstance<IIndexerService<T>>();

            try
            {
                var queueName = _appServiceSettings.QueueName(typeof(T));

                if (string.IsNullOrEmpty(queueName))
                {
                    return;
                }

                var messageCount = _cloudQueueService.GetQueueMessageCount(queueName);

                if (messageCount == 0)
                {
                    return;
                }

                var messages = _cloudQueueService.GetQueueMessages(queueName, messageCount)?.ToArray();

                if (messages != null && messages.Any())
                {
                    var latestMessage = messages.First();

                    var extraMessages = messages.Where(m => m != latestMessage).ToList();

                    // Delete all the messages except the first as they are not needed
                    _cloudQueueService.DeleteQueueMessages(queueName, extraMessages);

                    var indexTime = latestMessage.InsertionTime ?? DateTime.Now;

                    await indexerService.CreateScheduledIndex(indexTime).ConfigureAwait(false);

                    _cloudQueueService.DeleteQueueMessage(queueName, latestMessage);
                }
            }
            catch (Exception ex)
            {
                _log.Fatal("Something failed creating index: " + ex);
                throw;
            }
        }
    }
}