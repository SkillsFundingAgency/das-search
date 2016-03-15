namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Extensions;

    public class AzureQueueService : IGetMessageTimes, IClearQueue
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly CloudQueueService _cloudQueueService;

        public AzureQueueService(CloudQueueService cloudQueueService, IAppServiceSettings appServiceSettings)
        {
            _cloudQueueService = cloudQueueService;
            _appServiceSettings = appServiceSettings;
        }

        public void ClearQueue(string queuename)
        {
            var queue = _cloudQueueService.GetQueueReference(_appServiceSettings.ConnectionString, queuename);
            queue.GetMessages(10).ForEach(x => queue.DeleteMessage(x));
        }

        public IEnumerable<DateTime> GetInsertionTimes(string queuename)
        {
            var queue = _cloudQueueService.GetQueueReference(_appServiceSettings.ConnectionString, queuename);
            var cloudQueueMessages = queue.GetMessages(10);
            return cloudQueueMessages.OrderByDescending(x => x.InsertionTime).Select(x => x.InsertionTime?.DateTime ?? DateTime.Now);
        }
    }
}