using System;

namespace Sfa.Eds.Das.Indexer.Common.Helpers
{
    public interface IIndexerScheduler
    {
        void Schedule(Action action, int intervalMinutes);
    }
}