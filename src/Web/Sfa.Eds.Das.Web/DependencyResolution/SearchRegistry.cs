namespace Sfa.Eds.Das.Web.DependencyResolution
{
    using Sfa.Eds.Das.Web.Services;

    using StructureMap.Configuration.DSL;

    public class SearchRegistry : Registry
    {
        public SearchRegistry()
        {
            For<IMappingService>().Use<MappingService>();
        }
    }
}