using System;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public interface IProviderIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}