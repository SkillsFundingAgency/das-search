using System;

namespace Sfa.Eds.Indexer.Indexer.Infrastructure.Helpers
{
    public interface IIndexerScheduler
    {
        void Schedule(Action action, int intervalMinutes);
    }
}