namespace Sfa.Das.Sas.Web.Health
{
    using System.Configuration;

    public class HealthSettings : IHealthSettings
    {
        public string ApprenticeshipApiBaseUrl => ConfigurationManager.AppSettings["ApprenticeshipApiBaseUrl"];

        public string PostcodeUrl => ConfigurationManager.AppSettings["PostcodeUrl"];

        public string BuildId => ConfigurationManager.AppSettings["BuildId"];
    }
}