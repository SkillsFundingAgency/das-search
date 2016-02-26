using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.Common.Models;

namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    public interface IStandardHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexStandards(DateTime scheduledRefreshDateTime, IEnumerable<MetaDataItem> standards);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
        string GetIndexNameAndDateExtension(DateTime dateTime);
        Task<IEnumerable<MetaDataItem>> GetStandardsFromAzureAsync();    
        void UpdateMetadataRepositoryWithNewStandards();
        IEnumerable<MetaDataItem> GetStandardsMetaDataFromGit();
    }
}