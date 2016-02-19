using System;

namespace Sfa.Eds.Das.StandardIndexer.Services
{
    public interface IStandardIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}