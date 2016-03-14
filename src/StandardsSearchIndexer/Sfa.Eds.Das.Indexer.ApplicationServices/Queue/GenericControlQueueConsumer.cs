namespace Sfa.Eds.Das.Indexer.ApplicationServices.Queue
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Services;
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

        private readonly ILog _log;

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
            this._log = log;
        }

        public async Task CheckMessage<T>()
            where T : IIndexEntry
        {
            var indexerService = _container.GetInstance<IIndexerService<T>>();

            try
            {
                var queuename = _appServiceSettings.QueueName(typeof(T));
                var times = _cloudQueueService.GetInsertionTimes(queuename).ToList();

                if (times.Any())
                {
                    var time = times.FirstOrDefault();
                    await indexerService.CreateScheduledIndex(time).ConfigureAwait(false);
                }

                _clearQueue.ClearQueue(queuename);
            }
            catch (Exception ex)
            {
                _log.Fatal("Something failed creating index: " + ex);
                throw;
            }
        }
    }
}