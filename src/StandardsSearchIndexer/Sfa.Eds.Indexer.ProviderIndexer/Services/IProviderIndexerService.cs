using System;

namespace Sfa.Eds.Das.ProviderIndexer.Services
{
    public interface IProviderIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}