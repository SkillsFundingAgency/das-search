namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Extensions;

    public class AzureQueueService : IGetMessageTimes, IClearQueue
    {
        private readonly IAppServiceSettings _appServiceSettings;
        private readonly ILog _logger;
        private readonly CloudQueueService _cloudQueueService;

        public AzureQueueService(CloudQueueService cloudQueueService, IAppServiceSettings appServiceSettings, ILog logger)
        {
            _cloudQueueService = cloudQueueService;
            _appServiceSettings = appServiceSettings;
            _logger = logger;
        }

        public void ClearQueue(string queuename)
        {
            var queue = _cloudQueueService.GetQueueReference(_appServiceSettings.ConnectionString, queuename);
            queue.GetMessages(10).ForEach(x => queue.DeleteMessage(x));
        }

        public IEnumerable<DateTime> GetInsertionTimes(string queuename)
        {
            _logger.Trace($"ConnectionString: {_appServiceSettings.ConnectionString}, QueueName:{queuename}");

            var queue = _cloudQueueService.GetQueueReference(_appServiceSettings.ConnectionString, queuename);
            var cloudQueueMessages = queue.GetMessages(10);

            return cloudQueueMessages.OrderByDescending(x => x.InsertionTime).Select(x => x.InsertionTime?.DateTime ?? DateTime.Now);
        }
    }
}