namespace Sfa.Das.WebTest.Infrastructure.Settings
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
    }
}