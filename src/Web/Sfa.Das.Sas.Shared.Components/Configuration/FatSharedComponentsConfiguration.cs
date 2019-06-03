using System;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Shared.Components.Configuration
{
    public class FatSharedComponentsConfiguration : IFatConfigurationSettings, IPostcodeIOConfigurationSettings
    {
        public string FatApiBaseUrl { get; set; }
        public string SaveEmployerFavouritesUrl { get; set; }
        public Uri PostcodeUrl { get; set; }
        public Uri PostcodeTerminatedUrl { get; set; }
    }
}
