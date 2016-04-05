namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IGenericIndexerHelper<T>
    {
        Task IndexEntries(string indexName);

        bool DeleteOldIndexes(DateTime scheduledRefreshDateTime);

        bool IsIndexCorrectlyCreated(string indexName);

        bool CreateIndex(string indexName);

        void ChangeUnderlyingIndexForAlias(string newIndexName);
    }
}