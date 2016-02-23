namespace Sfa.Eds.Das.Indexer.Common.Settings
{
    public interface ICommonSettings
    {
        string SearchHost { get; }
        string WorkerRolePauseTime { get; }
        string StandardIndexesAlias { get; }
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string ConnectionString { get; }
        string QueueName { get; }
        string PauseTime { get; }
        string StandardJsonContainer { get; }
        string StandardPdfContainer { get; }
        string StandardContentType { get; }
    }
}