using Sfa.Eds.Das.Indexer.Common.Settings;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.Common.DependencyResolution
{
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IAzureSettings>().Use<AzureSettings>();
        }
    }
}