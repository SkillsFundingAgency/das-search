namespace Sfa.Das.Sas.Web.Health
{
    using StructureMap;

    public class HealthRegistry : Registry
    {
        public HealthRegistry()
        {
            For<IHealthService>().Use<HealthService>();
            For<IHealthSettings>().Use<HealthSettings>();
        }
    }
}
