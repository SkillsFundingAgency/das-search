namespace Sfa.Eds.Das.Indexer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenericIndexerHelper<T>
    {
        Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<T> entries);

        Task<ICollection<T>> LoadEntries();

        bool DeleteOldIndexes(DateTime scheduledRefreshDateTime);

        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);

        bool CreateIndex(DateTime scheduledRefreshDateTime);

        void SwapIndexes(DateTime scheduledRefreshDateTime);
    }
}