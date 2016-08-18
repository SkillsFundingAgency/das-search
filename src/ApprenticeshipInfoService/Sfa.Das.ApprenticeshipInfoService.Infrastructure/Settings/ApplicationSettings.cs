namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Azure;
    using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;

    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string ApprenticeshipIndexAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];

        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();

        public Uri SurveyUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyUrl"));

        public string CookieInfoBannerCookieName => ConfigurationManager.AppSettings["CookieInfoBannerCookieName"];

        public Uri PostcodeUrl => new Uri(CloudConfigurationManager.GetSetting("PostcodeUrl"));

        public string EnvironmentName => ConfigurationManager.AppSettings["EnvironmentName"];

        public string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];

        public Uri SatisfactionSourceUrl => new Uri(CloudConfigurationManager.GetSetting("SatisfactionSourceUrl"));

        public Uri AchievementRateUrl => new Uri(CloudConfigurationManager.GetSetting("AchievementRateUrl"));

        public Uri CookieImprovementUrl => new Uri(CloudConfigurationManager.GetSetting("CookieImprovementUrl"));

        public Uri CookieGoogleUrl => new Uri(CloudConfigurationManager.GetSetting("CookieGoogleUrl"));

        public Uri CookieApplicationInsightsUrl => new Uri(CloudConfigurationManager.GetSetting("CookieApplicationInsightsUrl"));

        public Uri CookieAboutUrl => new Uri(CloudConfigurationManager.GetSetting("CookieAboutUrl"));

        public Uri SurveyProviderUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyProviderUrl"));

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = CloudConfigurationManager.GetSetting("ElasticServerUrls").Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}
