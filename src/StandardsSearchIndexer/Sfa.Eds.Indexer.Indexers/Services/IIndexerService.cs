using System;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Indexer.Common.Services
{
    public interface IIndexerService
    {
        Task CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}