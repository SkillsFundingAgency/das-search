namespace Sfa.Das.ApprenticeshipInfoService.Core.Configuration
{
    using System;
    using System.Collections.Generic;

    public interface IConfigurationSettings
    {
        string ApprenticeshipIndexAlias { get; }

        string ProviderIndexAlias { get; }

        string BuildId { get; }

        IEnumerable<Uri> ElasticServerUrls { get; }

        Uri SurveyUrl { get; }

        string CookieInfoBannerCookieName { get; }

        Uri PostcodeUrl { get; }

        string EnvironmentName { get; }

        string ApplicationName { get; }

        Uri SatisfactionSourceUrl { get; }

        Uri CookieImprovementUrl { get; }

        Uri CookieGoogleUrl { get; }

        Uri CookieApplicationInsightsUrl { get; }

        Uri CookieAboutUrl { get; }

        Uri SurveyProviderUrl { get; }

        Uri AchievementRateUrl { get; }
    }
}
