using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Services;
using StructureMap.Configuration.DSL;

namespace Sfa.Das.Sas.Web.DependencyResolution
{
    public class SearchRegistry : Registry
    {
        public SearchRegistry()
        {
            For<IMappingService>().Use<MappingService>();
            For<IProviderViewModelFactory>().Use<ProviderViewModelFactory>();
            For<IListCollection<int>>().Use<CookieListCollection>();

            For<IHttpCookieFactory>().Use<HttpCookieFactory>();
            For<IShortlistStandardViewModelFactory>().Use<ShortlistStandardViewModelFactory>();
            For<IDashboardViewModelFactory>().Use<DashboardViewModelFactory>();
            For<IApprenticeshipViewModelFactory>().Use<ApprenticeshipViewModelFactory>();
        }
    }
}