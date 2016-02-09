using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Indexer.Indexers.Models;

namespace Sfa.Eds.Indexer.Indexers.Helpers
{
    public interface IProviderHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexProviders(DateTime scheduledRefreshDateTime, IEnumerable<Provider> providers);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
        string GetIndexNameAndDateExtension(DateTime dateTime);
        Task<IEnumerable<Provider>> GetProviders();
    }
}