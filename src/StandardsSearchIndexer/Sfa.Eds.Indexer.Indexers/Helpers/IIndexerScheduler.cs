using System;

namespace Sfa.Eds.Indexer.Common.Helpers
{
    public interface IIndexerScheduler
    {
        void Schedule(Action action, int intervalMinutes);
    }
}