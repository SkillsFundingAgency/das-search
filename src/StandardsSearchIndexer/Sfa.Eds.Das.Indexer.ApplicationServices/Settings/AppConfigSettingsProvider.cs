namespace Sfa.Eds.Das.Indexer.ApplicationServices.Settings
{
    using System.Configuration;
    using Core.Services;

    // TODO: LWA - Does this belong in this project?
    public class AppConfigSettingsProvider : IProvideSettings
    {
        private readonly IProvideSettings _baseSettings;

        public AppConfigSettingsProvider(IProvideSettings baseSettings)
        {
            _baseSettings = baseSettings;
        }

        public string GetSetting(string settingKey)
        {
            var setting = ConfigurationManager.AppSettings[settingKey];

            if (string.IsNullOrWhiteSpace(setting))
            {
                setting = TryBaseSettingsProvider(settingKey);
            }

            if (string.IsNullOrEmpty(setting))
            {
                throw new ConfigurationErrorsException($"Setting with key {settingKey} is missing");
            }

            return setting;
        }

        private string TryBaseSettingsProvider(string settingKey)
        {
            return _baseSettings.GetSetting(settingKey);
        }
    }
}