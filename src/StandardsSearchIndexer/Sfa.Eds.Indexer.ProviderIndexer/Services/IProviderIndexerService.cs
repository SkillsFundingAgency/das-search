using System;

namespace Sfa.Eds.Indexer.ProviderIndexer.Services
{
    public interface IProviderIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}