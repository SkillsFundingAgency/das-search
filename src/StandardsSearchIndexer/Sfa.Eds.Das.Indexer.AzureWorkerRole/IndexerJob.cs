namespace Sfa.Eds.Das.Indexer.AzureWorkerRole
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ApplicationServices.Services;
    using Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public class IndexerJob : IIndexerJob
    {
        private readonly IGenericControlQueueConsumer _controlQueueConsumer;

        public IndexerJob(IGenericControlQueueConsumer controlQueueConsumer)
        {
            _controlQueueConsumer = controlQueueConsumer;
        }

        public void Run()
        {
            var tasks = new List<Task> { _controlQueueConsumer.CheckMessage<IMaintainStandardIndex>(), _controlQueueConsumer.CheckMessage<IMaintainProviderIndex>() };

            Task.WaitAll(tasks.ToArray());
        }
    }
}