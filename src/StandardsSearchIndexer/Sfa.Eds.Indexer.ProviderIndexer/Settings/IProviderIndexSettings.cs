namespace Sfa.Eds.Das.ProviderIndexer.Settings
{
    public interface IProviderIndexSettings
    {
        string SearchHost { get; }
        string ActiveProvidersPath { get; }
        string ProviderIndexesAlias { get; }
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string ConnectionString { get; }
        string QueueName { get; }
        string PauseTime { get; }
    }
}