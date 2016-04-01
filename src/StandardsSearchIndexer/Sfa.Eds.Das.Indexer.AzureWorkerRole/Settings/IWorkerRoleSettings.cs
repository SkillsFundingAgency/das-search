namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings
{
    public interface IWorkerRoleSettings
    {
        string StorageConnectionString { get; }
        string WorkerRolePauseTime { get; }
    }
}