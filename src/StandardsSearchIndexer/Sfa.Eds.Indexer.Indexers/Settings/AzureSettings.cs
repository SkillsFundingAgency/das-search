using System.Configuration;

namespace Sfa.Eds.Das.Indexer.Common.Settings
{
    public class AzureSettings : IAzureSettings
    {
        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName => ConfigurationManager.AppSettings["ProviderQueueName"];
    }
}