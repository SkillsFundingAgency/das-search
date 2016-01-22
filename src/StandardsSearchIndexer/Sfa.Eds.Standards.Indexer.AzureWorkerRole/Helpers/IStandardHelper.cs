using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public interface IStandardHelper
    {
        bool CreateIndex(DateTime scheduledRefreshDateTime);
        Task IndexStandards(DateTime scheduledRefreshDateTime);
        void SwapIndexes(DateTime scheduledRefreshDateTime);
        bool IsIndexCorrectlyCreated();
    }
}