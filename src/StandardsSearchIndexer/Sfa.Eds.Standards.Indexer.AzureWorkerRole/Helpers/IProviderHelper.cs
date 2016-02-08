using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public interface IProviderHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexProviders(DateTime scheduledRefreshDateTime);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        void DeleteOldIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime);
    }
}