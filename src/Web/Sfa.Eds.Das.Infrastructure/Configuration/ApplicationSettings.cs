namespace Sfa.Eds.Das.Infrastructure.Configuration
{
    using System.Configuration;

    using Core.Configuration;

    public class ApplicationSettings : IConfigurationSettings
    {
        public string SearchHost => $"http://{this.ElasticServerIp}:{this.ElasticsearchPort}";

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];
    }
}