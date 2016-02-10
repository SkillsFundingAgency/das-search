using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Indexer.Indexer.Infrastructure.Models;

namespace Sfa.Eds.Indexer.ProviderIndexer.Helpers
{
    public interface IProviderHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexProviders(DateTime scheduledRefreshDateTime, List<Provider> providers);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
        string GetIndexNameAndDateExtension(DateTime dateTime);
        Task<List<Provider>> GetProviders();
    }
}