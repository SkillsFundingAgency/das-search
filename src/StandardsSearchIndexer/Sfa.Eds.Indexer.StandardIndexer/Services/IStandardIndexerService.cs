using System;

namespace Sfa.Eds.Indexer.StandardIndexer.Services
{
    public interface IStandardIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}