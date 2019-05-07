using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Shared.Components.Configuration
{
    public class FatSharedComponentsConfiguration : IFatConfigurationSettings
    {
        public string FatApiBaseUrl { get; set; }
        public string EmployerFavouritesUrl { get; set; }
    }
}
