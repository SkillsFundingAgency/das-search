using System;
using System.Threading.Tasks;

namespace Sfa.Eds.Indexer.Indexers.Helpers
{
    public interface IProviderHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexProviders(DateTime scheduledRefreshDateTime);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
        string GetIndexNameAndDateExtension(DateTime dateTime);
    }
}