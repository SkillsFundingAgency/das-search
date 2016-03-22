namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    public class ProviderRegistry : Registry
    {
        public ProviderRegistry()
        {
            For<IIndexSettings<IMaintainProviderIndex>>().Use<ProviderIndexSettings>();
            For<IGenericIndexerHelper<IMaintainProviderIndex>>().Use<ProviderIndexer>();
            For<IIndexerService<IMaintainProviderIndex>>().Use<IndexerService<IMaintainProviderIndex>>();
            For<IProviderFeatures>().Use<ProviderIndexSettings>();
        }
    }
}