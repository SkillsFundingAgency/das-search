using System;

namespace Sfa.Eds.Indexer.Indexers.Services
{
    public interface IStandardIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}