using System;
using System.Configuration;
using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Configuration
{
    public sealed class ApplicationSettings : IConfigurationSettings
    {
        public string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];

        public string EnvironmentName => ConfigurationManager.AppSettings["EnvironmentName"];
    }
}
