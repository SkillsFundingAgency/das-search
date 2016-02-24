using System;

namespace Sfa.Eds.Das.Indexer.Common.Settings
{
    public interface IAzureSettings
    {
        string ConnectionString { get; }

        string QueueName(Type type);
    }
}