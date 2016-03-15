namespace Sfa.Eds.Das.Indexer.ApplicationServices.Framework
{
    using System.Configuration;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public class FrameworkIndexSettings : IIndexSettings<FrameworkMetaData>
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string IndexesAlias => ConfigurationManager.AppSettings["FrameworkIndexesAlias"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName => ConfigurationManager.AppSettings["QueueName"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        private string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        private string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];
    }
}