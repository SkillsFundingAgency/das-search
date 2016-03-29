namespace Sfa.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    public class InfrastructureSettings : BaseSettings, IInfrastructureSettings
    {
        public string WorkerRolePauseTime => ConfigurationManager.AppSettings["WorkerRolePauseTime"];

        public string ApprenticeshipIndexAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public string StandardJsonContainer => ConfigurationManager.AppSettings["Standard.JsonContainer"];

        public string StandardPdfContainer => ConfigurationManager.AppSettings["Standard.PdfContainer"];

        public string StandardContentType => ConfigurationManager.AppSettings["Standard.ContentType"];

        public string CourseDirectoryUri => ConfigurationManager.AppSettings["CourseDirectoryUri"];

        public IEnumerable<Uri> ElasticServerUrls => GetElasticIPs("ElasticServerUrls");
    }
}