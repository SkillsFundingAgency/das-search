using Sfa.Eds.Das.ProviderIndexer.Helpers;
using Sfa.Eds.Das.ProviderIndexer.Services;
using Sfa.Eds.Das.ProviderIndexer.Settings;
using StructureMap;

namespace Sfa.Eds.Das.ProviderIndexer.DependencyResolution
{
    using Sfa.Eds.Das.ProviderIndexer.Clients;

    public class ProviderRegistry : Registry
    {
        public ProviderRegistry()
        {
            For<IProviderIndexerService>().Use<ProviderIndexerService>();
            For<IProviderHelper>().Use<ProviderHelper>();
            For<IProviderIndexSettings>().Use<ProviderIndexSettings>();
            For<ICourseDirectoryClient>().Use<StubCourseDirectoryClient>();
            For<IActiveProviderClient>().Use<FcsActiveProvidersClient>();
        }
    }
}