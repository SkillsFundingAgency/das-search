namespace Sfa.Das.Sas.Indexer.AzureWorkerRole.Settings
{
    public interface IWorkerRoleSettings
    {
        string StorageConnectionString { get; }
        string WorkerRolePauseTime { get; }
    }
}