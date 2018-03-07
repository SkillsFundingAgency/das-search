using Microsoft.Azure;

namespace Sfa.Das.Sas.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Sfa.Das.Sas.Core.Configuration;

    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string ApprenticeshipIndexAlias => CloudConfigurationManager.GetSetting("ApprenticeshipIndexAlias");

        public string ProviderIndexAlias => CloudConfigurationManager.GetSetting("ProviderIndexAlias");

        public string ApprenticeshipApiBaseUrl => CloudConfigurationManager.GetSetting("ApprenticeshipApiBaseUrl");

        public string BuildId => CloudConfigurationManager.GetSetting("BuildId");

        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();

        public Uri SurveyUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyUrl"));

        public string CookieInfoBannerCookieName => ConfigurationManager.AppSettings["CookieInfoBannerCookieName"];

        public Uri PostcodeUrl => new Uri(CloudConfigurationManager.GetSetting("PostcodeUrl"));

        public Uri PostcodeTerminatedUrl => new Uri(CloudConfigurationManager.GetSetting("PostcodeTerminatedUrl"));

        public string EnvironmentName => CloudConfigurationManager.GetSetting("EnvironmentName");

        public Uri SatisfactionSourceUrl => new Uri(CloudConfigurationManager.GetSetting("SatisfactionSourceUrl"));

        public Uri AchievementRateUrl => new Uri(CloudConfigurationManager.GetSetting("AchievementRateUrl"));

        public string ElasticsearchUsername => CloudConfigurationManager.GetSetting("ElasticsearchUsername");

        public string ElasticsearchPassword => CloudConfigurationManager.GetSetting("ElasticsearchPassword");

        public Uri CookieImprovementUrl => new Uri(CloudConfigurationManager.GetSetting("CookieImprovementUrl"));

        public Uri CookieGoogleUrl => new Uri(CloudConfigurationManager.GetSetting("CookieGoogleUrl"));

        public Uri CookieApplicationInsightsUrl => new Uri(CloudConfigurationManager.GetSetting("CookieApplicationInsightsUrl"));

        public Uri CookieAboutUrl => new Uri(CloudConfigurationManager.GetSetting("CookieAboutUrl"));

        public Uri SurveyProviderUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyProviderUrl"));

        public Uri ManageApprenticeshipFundsUrl => new Uri(CloudConfigurationManager.GetSetting("ManageApprenticeshipFundsUrl"));

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = CloudConfigurationManager.GetSetting("ElasticServerUrls").Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}