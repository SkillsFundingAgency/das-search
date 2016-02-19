using Sfa.Deds.Services;
using Sfa.Deds.Settings;
using StructureMap;

namespace Sfa.Deds.DependencyResolution
{
    public class DedsRegistry : Registry
    {
        public DedsRegistry()
        {
            For<ILarsClient>().Use<LarsClient>();
            For<ILarsSettings>().Use<LarsSettings>();
        }
    }
}