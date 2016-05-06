using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Infrastructure.Configuration
{
    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string ApprenticeshipIndexAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];

        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();

        public Uri SurveyUrl => new Uri(ConfigurationManager.AppSettings["SurveyUrl"]);

        // Will set the use secure cookies to true if no valid false value is found (secure by default) 
        public bool UseSecureCookies => !ConfigurationManager.AppSettings["SecureCookies"]
                                                             .Equals("false", StringComparison.InvariantCultureIgnoreCase);

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = ConfigurationManager.AppSettings["ElasticServerUrls"].Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}