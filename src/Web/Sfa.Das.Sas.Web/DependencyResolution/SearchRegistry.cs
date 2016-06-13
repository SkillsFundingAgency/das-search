using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Services;
using StructureMap.Configuration.DSL;

namespace Sfa.Das.Sas.Web.DependencyResolution
{
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    public class SearchRegistry : Registry
    {
        public SearchRegistry()
        {
            For<IMappingService>().Use<MappingService>();
            For<ICookieService>().Use<CookieService>();
            For<IProviderViewModelFactory>().Use<ProviderViewModelFactory>();
            For<IListCollection<int>>().Use<CookieListCollection>();

            For<IHttpCookieFactory>().Use<HttpCookieFactory>();
            For<IShortlistStandardViewModelFactory>().Use<ShortlistStandardViewModelFactory>();
            For<IDashboardViewModelFactory>().Use<DashboardViewModelFactory>();
            For<IApprenticeshipViewModelFactory>().Use<ApprenticeshipViewModelFactory>();

            For<IValidation>().Use<Validation>();
        }
    }
}