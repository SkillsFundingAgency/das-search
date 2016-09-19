namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class HealthSettings : IHealthSettings
    {
        public string Environment => ConfigurationManager.AppSettings["Environment"];

        public IEnumerable<Uri> ElasticsearchUrls => GetElasticSearchIps("ElasticServerUrls");

        public string LarsZipFileUrl => ConfigurationManager.AppSettings["LarsZipFileUrl"];

        public string CourseDirectoryUrl => ConfigurationManager.AppSettings["CourseDirectoryUrl"];

        private IEnumerable<Uri> GetElasticSearchIps(string configString)
        {
            var urlStrings = ConfigurationManager.AppSettings[configString].Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}