using StructureMap;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{

    using Core.Configuration;
    using Core.Logging;
    using Logging;

    public sealed class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, x.GetInstance<IConfigurationSettings>(), x.GetInstance<IRequestContext>())).AlwaysUnique();
            For<IConfigurationSettings>().Use<InfrastructureSettings>();
        }
    }
}