using System;
using System.Configuration;

namespace Sfa.Das.Sas.Web.Services
{
    using Extensions;

    public static class SettingsFinder
    {
        public static bool IsFeatureAvailable(string name)
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings[$"FeatureToggle.{name}"]);
        }

        public static bool IsNullOrEmpty(string settingsKey)
        {
            return ConfigurationManager.AppSettings[settingsKey].IsNullOrEmpty();
        }
    }
}