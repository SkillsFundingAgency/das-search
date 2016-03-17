using StructureMap.Configuration.DSL;

namespace Sfa.Das.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Core.Domain.Services;

    public sealed class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IStandardSearchService>().Use<StandardSearchService>();
            For<IProviderSearchService>().Use<ProviderSearchService>();
        }
    }
}
