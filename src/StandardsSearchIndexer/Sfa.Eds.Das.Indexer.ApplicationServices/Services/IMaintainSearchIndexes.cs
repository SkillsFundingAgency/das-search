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

        bool IndexContainsDocuments(string indexName);

        void CreateIndexAlias(string aliasName, string indexName);

        bool AliasExists(string aliasName);

        void SwapAliasIndex(string aliasName, string newIndexName);
    }
}
