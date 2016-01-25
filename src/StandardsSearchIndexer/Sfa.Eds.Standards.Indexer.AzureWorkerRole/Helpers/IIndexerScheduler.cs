using System;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public interface IIndexerScheduler
    {
        void Schedule(Action action, int intervalMinutes);
    }
}