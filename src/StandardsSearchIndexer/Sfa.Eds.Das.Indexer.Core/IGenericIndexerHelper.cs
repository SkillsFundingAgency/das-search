namespace Sfa.Eds.Das.Indexer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenericIndexerHelper<T>
    {
        Task IndexEntries(DateTime scheduledRefreshDateTime, ICollection<T> entries);
        ICollection<T> LoadEntries();
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
        string GetIndexNameAndDateExtension(DateTime dateTime);

        bool CreateIndex(DateTime scheduledRefreshDateTime);

        void SwapIndexes(DateTime scheduledRefreshDateTime);
    }
}