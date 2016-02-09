using System;

namespace Sfa.Eds.Indexer.Indexers.Helpers
{
    public interface IIndexerScheduler
    {
        void Schedule(Action action, int intervalMinutes);
    }
}