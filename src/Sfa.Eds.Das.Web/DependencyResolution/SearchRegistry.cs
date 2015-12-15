namespace Sfa.Eds.Das.Web.DependencyResolution {
    using Services;
    using StructureMap.Configuration.DSL;

    public class SearchRegistry : Registry {

        public SearchRegistry() {
            For<ISearchForStandards>().Use<StubSearchService>();
        }
    }
}