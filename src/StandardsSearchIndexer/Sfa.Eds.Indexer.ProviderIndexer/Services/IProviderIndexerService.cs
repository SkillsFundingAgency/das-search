using System;

namespace Sfa.Eds.ProviderIndexer.Services
{
    public interface IProviderIndexerService
    {
        void CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}