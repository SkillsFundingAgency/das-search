namespace Sfa.Eds.Das.Web.DependencyResolution
{
    using Services;
    using StructureMap.Configuration.DSL;

    public class LocationRegistry : Registry
    {
        public LocationRegistry()
        {
            For<ILocationService>().Use<LocationService>();
        }
    }
}