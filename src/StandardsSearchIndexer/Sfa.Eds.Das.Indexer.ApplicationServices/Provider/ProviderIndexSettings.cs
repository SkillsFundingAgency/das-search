namespace Sfa.Eds.Das.Indexer.ApplicationServices.Provider
{
    using System.Configuration;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;

    public class ProviderIndexSettings : IIndexSettings<Provider>, IProviderFeatures
    {
        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];

        public string ActiveProvidersPath => ConfigurationManager.AppSettings["VstsProvidersFolderPath"];

        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string IndexesAlias => ConfigurationManager.AppSettings["ProviderIndexesAlias"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName => ConfigurationManager.AppSettings["ProviderQueueName"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public bool FilterInactiveProviders => bool.Parse(ConfigurationManager.AppSettings["Feature.FilterInactiveProviders"] ?? "false");
        
        public string StandardProviderDocumentType => ConfigurationManager.AppSettings["StandardProviderDocumentType"];

        public string FrameworkProviderDocumentType => ConfigurationManager.AppSettings["FrameworkProviderDocumentType"];
    }
}