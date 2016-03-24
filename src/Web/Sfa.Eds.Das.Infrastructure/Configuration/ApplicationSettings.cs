namespace Sfa.Eds.Das.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Core.Configuration;

    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string SearchHost => $"http://{ElasticServerIp}:{ElasticsearchPort}";

        public string ApprenticeshipIndexAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];

        public string ElasticsearchPort => ConfigurationManager.AppSettings["ElasticsearchPort"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];

        public IEnumerable<Uri> ElasticServerIps => GetElasticSearchIps();

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var ips = ConfigurationManager.AppSettings["ElasticServerIps"].Split('|');
            return ips.Select(ip => new Uri($"http://{ip}:9200"));
        }
    }
}