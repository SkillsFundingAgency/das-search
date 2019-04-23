using System;
using System.Collections.Generic;
using System.Text;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Shared.Components.Configuration
{
    public class FatSharedComponentsConfiguration : IFatConfigurationSettings
    {
        public virtual string FatApiBaseUrl { get; set; }
    }
}
