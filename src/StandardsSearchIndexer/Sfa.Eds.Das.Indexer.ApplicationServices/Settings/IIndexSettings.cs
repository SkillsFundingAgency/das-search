namespace Sfa.Eds.Das.Indexer.ApplicationServices.Settings
{
    public interface IIndexSettings<T>
    {
        string SearchHost { get; }
        string IndexesAlias { get; }
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string ConnectionString { get; }
        string QueueName { get; }
        string PauseTime { get; }
    }
}