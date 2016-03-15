namespace Sfa.Eds.Das.Indexer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenericIndexerHelper<T>
    {
        Task IndexEntries(string indexName, ICollection<T> entries);

        Task<ICollection<T>> LoadEntries();

        bool DeleteOldIndexes(DateTime scheduledRefreshDateTime);

        bool IsIndexCorrectlyCreated(string indexName);

        bool CreateIndex(string indexName);

        void SwapIndexes(string newIndexName);
    }
}