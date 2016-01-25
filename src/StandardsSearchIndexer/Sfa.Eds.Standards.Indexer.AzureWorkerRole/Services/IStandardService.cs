using System;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public interface IStandardService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}