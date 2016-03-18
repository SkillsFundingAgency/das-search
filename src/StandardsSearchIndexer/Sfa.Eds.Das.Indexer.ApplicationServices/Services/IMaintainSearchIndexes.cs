namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System;
    using Sfa.Eds.Das.Indexer.Common.Models;

    public interface IMaintainSearchIndexes
    {
        bool IndexExists(string indexName);

        bool DeleteIndex(string indexName);

        bool DeleteIndexes(Func<string, bool> indexNameMatch);

        void CreateIndex(string indexName);

        bool IndexContainsDocuments<T>(string indexName)
            where T : class, IIndexEntry;

        void CreateIndexAlias(string aliasName, string indexName);

        bool AliasExists(string aliasName);

        void SwapAliasIndex(string aliasName, string newIndexName);
    }
}
