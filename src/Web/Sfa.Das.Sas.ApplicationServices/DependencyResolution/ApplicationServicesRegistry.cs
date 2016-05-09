using Sfa.Das.Sas.ApplicationServices.Settings;
using StructureMap.Configuration.DSL;

namespace Sfa.Das.Sas.ApplicationServices.DependencyResolution
{
    public sealed class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IApprenticeshipSearchService>().Use<ApprenticeshipSearchService>();
            For<IProviderSearchService>().Use<ProviderSearchService>();
            For<IPaginationSettings>().Use<PaginationSettings>();
        }
    }
}
