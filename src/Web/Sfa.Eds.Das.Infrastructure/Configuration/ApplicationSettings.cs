namespace Sfa.Eds.Das.Infrastructure.Configuration
{
    using System;
    using System.Configuration;

    using Core.Configuration;

    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];
    }
}