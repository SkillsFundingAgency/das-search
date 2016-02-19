using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Sfa.Eds.Das.Indexer.Common.AzureAbstractions;
using Sfa.Eds.Das.Indexer.Common.Settings;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.Common.Services
{
    public class GenericControlQueueConsumer : IGenericControlQueueConsumer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ICloudQueueService _cloudQueueService;
        private readonly IContainer _container;

        private readonly IAzureSettings _azureSettings;

        public GenericControlQueueConsumer(
            IAzureSettings azureSettings,
            ICloudQueueService cloudQueueService,
            IContainer container)
        {
            _azureSettings = azureSettings;
            _cloudQueueService = cloudQueueService;
            _container = container;
        }

        public Task CheckMessage<T>() where T : IIndexerService
        {
            var indexerService = _container.GetInstance<T>();
            return Task.Run(() =>
            {
                try
                {
                    var queue = _cloudQueueService.GetQueueReference(_azureSettings.ConnectionString, _azureSettings.QueueName);
                    var cloudQueueMessages = queue.GetMessages(10);
                    var messages = cloudQueueMessages.OrderByDescending(x => x.InsertionTime);

                    if (messages.Any())
                    {
                        var message = messages.FirstOrDefault();
                        if (message != null)
                        {
                            indexerService.CreateScheduledIndex(message.InsertionTime?.DateTime ?? DateTime.Now);
                        }
                    }

                    foreach (var cloudQueueMessage in messages)
                    {
                        queue.DeleteMessage(cloudQueueMessage);
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal("Something failed creating index: " + ex);
                    throw;
                }
            });
        }
    }
}