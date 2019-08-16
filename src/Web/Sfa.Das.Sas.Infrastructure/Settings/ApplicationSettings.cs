namespace Sfa.Das.Sas.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sfa.Das.Sas.Core.Configuration;

    //TODO: LWA - Do we need this file?
    // public sealed class ApplicationSettings
    // {
    //     public Uri SurveyUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyUrl"));

    //     public string CookieInfoBannerCookieName => ConfigurationManager.AppSettings["CookieInfoBannerCookieName"];

    //     public Uri PostcodeUrl => new Uri(CloudConfigurationManager.GetSetting("PostcodeUrl"));

    //     public Uri PostcodeTerminatedUrl => new Uri(CloudConfigurationManager.GetSetting("PostcodeTerminatedUrl"));

    //     public string EnvironmentName => CloudConfigurationManager.GetSetting("EnvironmentName");

    //     public Uri SatisfactionSourceUrl => new Uri(CloudConfigurationManager.GetSetting("SatisfactionSourceUrl"));

    //     public Uri AchievementRateUrl => new Uri(CloudConfigurationManager.GetSetting("AchievementRateUrl"));

    //     public Uri CookieImprovementUrl => new Uri(CloudConfigurationManager.GetSetting("CookieImprovementUrl"));

    //     public Uri CookieGoogleUrl => new Uri(CloudConfigurationManager.GetSetting("CookieGoogleUrl"));

    //     public Uri CookieApplicationInsightsUrl => new Uri(CloudConfigurationManager.GetSetting("CookieApplicationInsightsUrl"));

    //     public Uri CookieAboutUrl => new Uri(CloudConfigurationManager.GetSetting("CookieAboutUrl"));

    //     public Uri SurveyProviderUrl => new Uri(CloudConfigurationManager.GetSetting("SurveyProviderUrl"));

    //     public Uri ManageApprenticeshipFundsUrl => new Uri(CloudConfigurationManager.GetSetting("ManageApprenticeshipFundsUrl"));

    //     public IEnumerable<long> HideAboutProviderForUkprns => GetHideAboutProviderUrkprns();

    //     private IEnumerable<long> GetHideAboutProviderUrkprns()
    //     {
    //         return
    //             CloudConfigurationManager.GetSetting("HideAboutProviderForUkprns")
    //                 .Split(',')
    //                 .Select(m => m.Trim())
    //                 .Where(m => System.Text.RegularExpressions.Regex.IsMatch(m, "^[0-9]{1,18}$"))
    //                 .Where(m => !string.IsNullOrEmpty(m))
    //                 .Select(m => long.Parse(m));
    //     }
    // }
}