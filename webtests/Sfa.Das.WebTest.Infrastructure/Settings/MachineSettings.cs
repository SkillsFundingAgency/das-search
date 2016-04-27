namespace Sfa.Das.WebTest.Infrastructure.Settings
{
    using System;
    using System.Globalization;

    public sealed class MachineSettings : IProvideSettings
    {
        public string GetSetting(string settingKey)
        {
            return Environment.GetEnvironmentVariable($"DAS_{settingKey.ToUpper(CultureInfo.InvariantCulture)}", EnvironmentVariableTarget.User);
        }
    }
}