namespace Sfa.Eds.Das.ProviderIndexer.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.Common.Helpers;
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Common.Services;
    using Sfa.Eds.Das.Indexer.Common.Settings;
    using Sfa.Eds.Das.ProviderIndexer.Clients;
    using Sfa.Eds.Das.ProviderIndexer.Helpers;
    using Sfa.Eds.Das.ProviderIndexer.Models;
    using Sfa.Eds.Das.ProviderIndexer.Settings;

    using StructureMap;

    public class ProviderRegistry : Registry
    {
        public ProviderRegistry()
        {
            For<IIndexSettings<Provider>>().Use<ProviderIndexSettings>();
            For<ICourseDirectoryClient>().Use<StubCourseDirectoryClient>();
            For<IActiveProviderClient>().Use<FcsActiveProvidersClient>();
            For<IGenericIndexerHelper<Provider>>().Use<ProviderHelper>();
            For<IIndexerService<Provider>>().Use<IndexerService<Provider>>();
        }
    }
}