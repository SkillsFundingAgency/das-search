namespace Sfa.Das.Sas.ApplicationServices.Settings
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
    }
}