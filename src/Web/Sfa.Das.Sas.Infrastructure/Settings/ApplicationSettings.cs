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
        public string ApprenticeshipIndexAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string ProviderIndexAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string ApprenticeshipApiBaseUrl => ConfigurationManager.AppSettings["ApprenticeshipApiBaseUrl"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];

        public IEnumerable<Uri> ElasticServerUrls => GetElasticSearchIps();

        public Uri SurveyUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyUrl"));

        public string CookieInfoBannerCookieName => ConfigurationManager.AppSettings["CookieInfoBannerCookieName"];

        public Uri PostcodeUrl => new Uri(CloudConfigurationManager.GetSetting("PostcodeUrl"));

        public string EnvironmentName => ConfigurationManager.AppSettings["EnvironmentName"];

        public string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];

        public Uri SatisfactionSourceUrl => new Uri(CloudConfigurationManager.GetSetting("SatisfactionSourceUrl"));

        public Uri AchievementRateUrl => new Uri(CloudConfigurationManager.GetSetting("AchievementRateUrl"));

        public string ElasticsearchUsername => ConfigurationManager.AppSettings["ElasticsearchUsername"];

        public string ElasticsearchPassword => ConfigurationManager.AppSettings["ElasticsearchPassword"];

        public Uri CookieImprovementUrl => new Uri(CloudConfigurationManager.GetSetting("CookieImprovementUrl"));

        public Uri CookieGoogleUrl => new Uri(CloudConfigurationManager.GetSetting("CookieGoogleUrl"));

        public Uri CookieApplicationInsightsUrl => new Uri(CloudConfigurationManager.GetSetting("CookieApplicationInsightsUrl"));

        public Uri CookieAboutUrl => new Uri(CloudConfigurationManager.GetSetting("CookieAboutUrl"));

        public Uri SurveyProviderUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyProviderUrl"));

        public Uri ManageApprenticeshipFundsUrl => new Uri(ConfigurationManager.AppSettings["ManageApprenticeshipFundsUrl"]);

        private IEnumerable<Uri> GetElasticSearchIps()
        {
            var urlStrings = CloudConfigurationManager.GetSetting("ElasticServerUrls").Split(',');
            return urlStrings.Select(url => new Uri(url));
        }
    }
}