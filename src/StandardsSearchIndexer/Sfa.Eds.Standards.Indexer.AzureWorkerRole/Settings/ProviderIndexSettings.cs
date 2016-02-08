using System.Configuration;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings
{
    public class ProviderIndexSettings : IProviderIndexSettings
    {
        public string SearchHost => ConfigurationManager.AppSettings["SearchHost"];

        public string ProviderIndexesAlias => ConfigurationManager.AppSettings["ProviderIndexesAlias"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => ConfigurationManager.AppSettings["ConnectionString"];

        public string QueueName => ConfigurationManager.AppSettings["ProviderQueueName"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];
    }
}