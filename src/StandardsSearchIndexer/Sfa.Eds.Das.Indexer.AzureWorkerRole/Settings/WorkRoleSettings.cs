namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings
{
    using System;
    using System.Configuration;

    public class WorkRoleSettings : IWorkerRoleSettings
    {
        public string WorkerRolePauseTime => ConfigurationManager.AppSettings["WorkerRolePauseTime"];

        public string StorageConnectionString => ConfigurationManager.AppSettings["StorageConnectionString"];
    }
}