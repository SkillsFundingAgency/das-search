using System;

namespace Sfa.Eds.Das.Indexer.Common.Settings
{
    public interface IAzureSettings
    {
        string ConnectionString { get; }
        string StandardQueueName { get; }
        string ProviderQueueName { get; }

        string QueueName(Type type);
    }
}