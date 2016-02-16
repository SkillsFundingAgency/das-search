using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Indexer.Indexer.Infrastructure.Models;

namespace Sfa.Eds.Indexer.StandardIndexer.Helpers
{
    public interface IStandardHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexStandards(DateTime scheduledRefreshDateTime, IEnumerable<JsonMetadataObject> standards);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
        string GetIndexNameAndDateExtension(DateTime dateTime);
        Task<IEnumerable<JsonMetadataObject>> GetStandardsFromAzureAsync();
    }
}