using System.Configuration;

namespace Sfa.Eds.Indexer.Settings.Settings
{
    public class StandardIndexSettings : IStandardIndexSettings
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string SearchEndpointConfigurationName => ConfigurationManager.AppSettings["SearchEndpointConfigurationName"];

        public string DatasetName => ConfigurationManager.AppSettings["DatasetName"];

        public string StandardDescriptorName => ConfigurationManager.AppSettings["StandardDescriptorName"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName => ConfigurationManager.AppSettings["QueueName"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public string StandardJsonContainer => ConfigurationManager.AppSettings["Standard.JsonContainer"];

        public string StandardPdfContainer => ConfigurationManager.AppSettings["Standard.PdfContainer"];

        public string StandardContentType => ConfigurationManager.AppSettings["Standard.ContentType"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];
    }
}