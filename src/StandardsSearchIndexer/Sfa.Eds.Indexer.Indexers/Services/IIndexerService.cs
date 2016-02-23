using System;

namespace Sfa.Eds.Das.Indexer.Common.Services
{
    public interface IIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}