using System;
using System.Configuration;

namespace Sfa.Das.Sas.Web.Services
{
    public static class SettingsFinder
    {
        public static bool IsFeatureAvailable(string name)
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings[$"FeatureToggle.{name}"]);
        }
    }
}