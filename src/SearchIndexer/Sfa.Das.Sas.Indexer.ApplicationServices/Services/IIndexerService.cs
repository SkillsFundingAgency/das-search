using System;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Services
{
    public interface IIndexerService<T>
    {
        Task CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}