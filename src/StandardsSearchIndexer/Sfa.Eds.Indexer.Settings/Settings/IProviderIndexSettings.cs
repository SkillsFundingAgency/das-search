namespace Sfa.Eds.Indexer.Settings.Settings
{
    public interface IProviderIndexSettings
    {
        string SearchHost { get; }
        string ProviderIndexesAlias { get; }
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string ConnectionString { get; }
        string QueueName { get; }
        string PauseTime { get; }
    }
}