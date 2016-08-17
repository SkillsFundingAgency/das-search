namespace Sfa.Das.Sas.Infrastructure
{
    using Core.Configuration;

    using ConfigurationManager = System.Configuration.ConfigurationManager;

    public sealed class InfrastructureSettings : IConfigurationSettings
    {
        public string EnvironmentName => ConfigurationManager.AppSettings["EnvironmentName"];

        public string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];
    }
}