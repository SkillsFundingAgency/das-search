namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Configuration;
    using System.Diagnostics;

    public class BaseSettings
    {
        public string GetSetting()
        {
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            var settingKey = method.Name.Split('_')[1];

            return GetSetting(settingKey);
        }

        public int GetSetting(string settingKey, int defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[settingKey];
            int parsedValue;

            if (int.TryParse(setting, out parsedValue))
            {
                return parsedValue;
            }

            return defaultValue;
        }

        public string GetSetting(string settingKey)
        {
            var setting = ConfigurationManager.AppSettings[settingKey];
            if (string.IsNullOrEmpty(setting))
            {
                throw new ConfigurationErrorsException($"Setting with key {settingKey} is missing");
            }

            return setting;
        }
    }
}