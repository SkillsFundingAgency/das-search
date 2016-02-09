using System;

namespace Sfa.Eds.Indexer.Indexers.Services
{
    public interface IProviderIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}