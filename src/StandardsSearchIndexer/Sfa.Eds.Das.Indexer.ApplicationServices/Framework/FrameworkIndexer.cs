namespace Sfa.Eds.Das.Indexer.ApplicationServices.Framework
{
    using System;
    using System.Threading.Tasks;

    using Core;
    using Core.Models.Framework;
    using Core.Services;

    public sealed class FrameworkIndexer : IGenericIndexerHelper<FrameworkMetaData>
    {
        private readonly ILog _log;

        public FrameworkIndexer(ILog log)
        {
            _log = log;
        }

        public async Task IndexEntries(string indexName)
        {
            throw new NotImplementedException("Indexing of frameworks is done in StandardIndexer");
        }

        public bool DeleteOldIndexes(DateTime scheduledRefreshDateTime)
        {
            throw new NotImplementedException("Deleting of index is done in StandardIndexer");
        }

        public bool IsIndexCorrectlyCreated(string indexName)
        {
            throw new NotImplementedException("This is done in StandardIndexer");
        }

        public bool CreateIndex(string indexName)
        {
            throw new NotImplementedException("Creation of index is done in StandardIndexer");
        }

        public void SwapIndexes(string newIndexName)
        {
            throw new NotImplementedException("Swpping indecies is done in StandardIndexer");
        }
    }
}
