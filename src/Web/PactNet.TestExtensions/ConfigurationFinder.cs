using System;
using System.Configuration;

namespace PactNet.TestExtensions
{
    internal static class ConfigurationFinder
    {
        public static string RequiredSetting(string key)
        {
            var setting = OptionalSetting(key);
            if (string.IsNullOrEmpty(setting))
            {
                throw new SettingsPropertyNotFoundException($"Missing the {key} as an environment variable or in the app.config");
            }

            return setting;
        }

        public static string OptionalSetting(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? ConfigurationManager.AppSettings[key];
        }

    }
}