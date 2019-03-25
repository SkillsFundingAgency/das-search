using Microsoft.Azure;

namespace Sfa.Das.Sas.Infrastructure.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Sfa.Das.Sas.Core.Configuration;

    public sealed class ApiSettings : IApiConfigurationSettings
    {
        public string ApprenticeshipApiBaseUrl => CloudConfigurationManager.GetSetting("ApprenticeshipApiBaseUrl");
    }

}