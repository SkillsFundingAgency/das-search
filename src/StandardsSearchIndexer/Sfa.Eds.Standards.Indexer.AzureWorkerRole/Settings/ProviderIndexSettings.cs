using System.Configuration;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings
{
    public class ProviderIndexSettings : IProviderIndexSettings
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string ProviderIndexesAlias => ConfigurationManager.AppSettings["ProviderIndexesAlias"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName => ConfigurationManager.AppSettings["ProviderQueueName"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];
    }
}