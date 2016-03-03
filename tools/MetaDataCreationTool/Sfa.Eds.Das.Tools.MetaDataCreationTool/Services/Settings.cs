namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System;
    using System.Configuration;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public class Settings : ISettings
    {
        public string StandardsUrl => GetSetting("StandardsUrl");
        public string JsonFilesDestination  => this.GetSetting("jsonFilesDestination");

        public string LarsZipFileUrl => GetSetting("LarsZipFileUrl");

        public string WorkingFolder => this.GetSetting("WorkingFolder");

        public string CsvFile => GetSetting("CsvFile");

        public int MaxStandards => GetSetting<int>("MaxStandards", int.MaxValue);

        private int GetSetting<T>(string settingKey, int defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[settingKey];
            int parsedValue;
            if(int.TryParse(setting, out parsedValue))
            {
                return parsedValue;
            }
            return defaultValue;
        }

        private string GetSetting(string settingKey)
        {
            var setting = ConfigurationManager.AppSettings[settingKey];
            if (string.IsNullOrEmpty(setting))
                throw new ConfigurationErrorsException($"Setting with key {settingKey} is missing");
            return setting;
        }
    }
}