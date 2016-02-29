using System;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Indexer.Common.Services
{
    public interface IIndexerService
    {
        Task CreateScheduledIndex(DateTime scheduledRefreshDateTime);

    }

    public interface IIndexerService<T>
    {
        Task CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}