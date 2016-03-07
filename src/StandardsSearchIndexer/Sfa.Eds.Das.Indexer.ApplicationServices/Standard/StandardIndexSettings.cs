using System.Configuration;

namespace Sfa.Eds.Das.StandardIndexer.Settings
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Common.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public class StandardIndexSettings : IStandardIndexSettings, IIndexSettings<MetaDataItem>
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string IndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

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