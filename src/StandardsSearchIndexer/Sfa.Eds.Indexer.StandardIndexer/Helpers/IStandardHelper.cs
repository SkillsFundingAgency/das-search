using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Indexer.Common.Models;

namespace Sfa.Eds.StandardIndexer.Helpers
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