using Microsoft.Azure;

namespace Sfa.Das.Sas.Infrastructure.Settings
{

    using Sfa.Das.Sas.Core.Configuration;

    public sealed class FatSettings : IFatConfigurationSettings
    {
        public string FatApiBaseUrl => CloudConfigurationManager.GetSetting("ApprenticeshipApiBaseUrl");
        public string EmployerFavouritesUrl => throw new System.NotImplementedException();
    }
}