using System;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public interface IStandardIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}