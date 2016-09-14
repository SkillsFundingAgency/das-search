namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class HealthSettings : IHealthSettings
    {
        public string Environment => ConfigurationManager.AppSettings["Environment"];

        public IEnumerable<Uri> ElasticsearchUrls => GetElasticSearchIps("ElasticsearchUrl");

        private IEnumerable<Uri> GetElasticSearchIps(string configString)
        {
            var urlStrings = ConfigurationManager.AppSettings[configString].Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }

    public interface IHealthSettings
    {
        string Environment { get; }

        IEnumerable<Uri> ElasticsearchUrls { get; }
    }
}
