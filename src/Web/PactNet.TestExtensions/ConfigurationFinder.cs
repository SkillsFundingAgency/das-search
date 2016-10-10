using System;
using System.Configuration;

namespace PactNet.TestExtensions
{
    internal static class ConfigurationFinder
    {
        public static string Setting(string key)
        {
            var setting = Environment.GetEnvironmentVariable(key) ?? ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(setting))
            {
                throw new SettingsPropertyNotFoundException($"Missing the {key} as an environment variable or in the app.config");
            }

            return setting;
        }
    }
}