using StructureMap.Configuration.DSL;

namespace Sfa.Das.ApplicationServices.DependencyResolution
{
    public sealed class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IStandardSearchService>().Use<StandardSearchService>();
            For<IProviderSearchService>().Use<ProviderSearchService>();
        }
    }
}
