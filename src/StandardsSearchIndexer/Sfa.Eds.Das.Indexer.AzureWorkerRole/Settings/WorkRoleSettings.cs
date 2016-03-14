namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings
{
    using System.Configuration;

    public class WorkRoleSettings : IWorkerRoleSettings
    {
        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string WorkerRolePauseTime => ConfigurationManager.AppSettings["WorkerRolePauseTime"];
    }
}