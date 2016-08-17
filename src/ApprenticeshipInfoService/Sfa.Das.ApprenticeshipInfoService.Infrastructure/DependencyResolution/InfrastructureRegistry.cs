using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;
using Sfa.Das.ApprenticeshipInfoService.Infrastructure.Configuration;
using Sfa.Das.ApprenticeshipInfoService.Infrastructure.Logging;
using StructureMap;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.DependencyResolution
{
    public sealed class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, x.GetInstance<IConfigurationSettings>(), x.GetInstance<IRequestContext>())).AlwaysUnique();
            For<IConfigurationSettings>().Use<ApplicationSettings>();
        }
    }
}
