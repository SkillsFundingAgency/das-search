namespace Sfa.Das.Sas.Web.Services
{
    using Extensions;
    using Microsoft.Azure;

    public static class SettingsFinder
    {
        public static bool IsNullOrEmpty(string settingsKey)
        {
            return CloudConfigurationManager.GetSetting(settingsKey).IsNullOrEmpty();
        }
    }
}