namespace Sfa.Das.ApprenticeshipInfoService.Core.Configuration
{
    public interface IConfigurationSettings
    {
        string EnvironmentName { get; }

        string ApplicationName { get; }
    }
}
