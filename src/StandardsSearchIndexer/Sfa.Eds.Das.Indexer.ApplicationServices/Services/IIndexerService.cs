namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IIndexerService<T>
    {
        Task CreateScheduledIndex(DateTime scheduledRefreshDateTime);
    }
}