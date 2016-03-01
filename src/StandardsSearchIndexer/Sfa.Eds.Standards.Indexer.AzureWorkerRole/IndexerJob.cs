namespace Sfa.Eds.Das.Indexer.AzureWorkerRole
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Common.Services;
    using Sfa.Eds.Das.ProviderIndexer.Models;

    public class IndexerJob : IIndexerJob
    {
        private readonly IGenericControlQueueConsumer _controlQueueConsumer;

        public IndexerJob(IGenericControlQueueConsumer controlQueueConsumer)
        {
            _controlQueueConsumer = controlQueueConsumer;
        }

        public void Run()
        {
            var tasks = new List<Task> { _controlQueueConsumer.CheckMessage<MetaDataItem>(), _controlQueueConsumer.CheckMessage<Provider>() };

            Task.WaitAll(tasks.ToArray());
        }
    }
}