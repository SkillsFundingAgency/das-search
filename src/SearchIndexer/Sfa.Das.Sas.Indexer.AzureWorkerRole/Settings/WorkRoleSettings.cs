using System.Configuration;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole.Settings
{
    public class WorkRoleSettings : IWorkerRoleSettings
    {
        public string WorkerRolePauseTime => ConfigurationManager.AppSettings["WorkerRolePauseTime"];

        public string StorageConnectionString => ConfigurationManager.AppSettings["StorageConnectionString"];
    }
}