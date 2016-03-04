namespace Sfa.Eds.Das.Indexer.ApplicationServices.Queue
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Common.Models;

    using StructureMap;

    public class GenericControlQueueConsumer : IGenericControlQueueConsumer
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly IClearQueue _clearQueue;

        private readonly IGetMessageTimes _cloudQueueService;

        private readonly IContainer _container;

        private readonly ILog Log;

        public GenericControlQueueConsumer(
            IAppServiceSettings appServiceSettings,
            IGetMessageTimes cloudQueueService,
            IClearQueue clearQueue,
            IContainer container,
            ILog log)
        {
            _appServiceSettings = appServiceSettings;
            _cloudQueueService = cloudQueueService;
            _clearQueue = clearQueue;
            _container = container;
            Log = log;
        }

        public Task CheckMessage<T>() where T : IIndexEntry
        {
            var indexerService = _container.GetInstance<IIndexerService<T>>();
            return Task.Run(
                async () =>
                    {
                        try
                        {
                            var queuename = _appServiceSettings.QueueName(typeof(T));
                            var times = _cloudQueueService.GetInsertionTimes(queuename);

                            if (times.Any())
                            {
                                var time = times.FirstOrDefault();
                                if (time != null)
                                {
                                    await indexerService.CreateScheduledIndex(time).ConfigureAwait(false);
                                }
                            }

                            _clearQueue.ClearQueue(queuename);
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