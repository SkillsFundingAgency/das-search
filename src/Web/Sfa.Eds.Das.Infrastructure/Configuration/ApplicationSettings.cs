namespace Sfa.Eds.Das.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Core.Configuration;

    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string ApprenticeshipIndexAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];

        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();

        public Uri SurveyUrl => new Uri(ConfigurationManager.AppSettings["SurveyUrl"]);

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = ConfigurationManager.AppSettings["ElasticServerUrls"].Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}