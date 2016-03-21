namespace Sfa.Infrastructure.Settings
{
    using System.Configuration;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    public class InfrastructureSettings : BaseSettings, IInfrastructureSettings
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string WorkerRolePauseTime => ConfigurationManager.AppSettings["WorkerRolePauseTime"];

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName => ConfigurationManager.AppSettings["QueueName"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public string StandardJsonContainer => ConfigurationManager.AppSettings["Standard.JsonContainer"];

        public string StandardPdfContainer => ConfigurationManager.AppSettings["Standard.PdfContainer"];

        public string StandardContentType => ConfigurationManager.AppSettings["Standard.ContentType"];

        public string CourseDirectoryUri => ConfigurationManager.AppSettings["CourseDirectoryUri"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];
    }
}