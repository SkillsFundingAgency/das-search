namespace Sfa.Das.Sas.Core.Configuration
{
    public interface IConfigurationSettings
    {
        string ApplicationName { get; }

        string EnvironmentName { get; }
    }
}