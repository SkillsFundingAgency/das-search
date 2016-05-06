using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Web.Collections;
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
        }
    }
}