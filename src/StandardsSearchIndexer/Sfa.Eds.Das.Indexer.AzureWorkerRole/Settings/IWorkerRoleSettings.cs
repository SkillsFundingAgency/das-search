namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings
{
    public interface IWorkerRoleSettings
    {
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string WorkerRolePauseTime { get; }
    }
}