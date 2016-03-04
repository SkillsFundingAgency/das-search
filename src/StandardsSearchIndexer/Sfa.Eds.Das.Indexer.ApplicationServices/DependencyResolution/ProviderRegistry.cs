namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    public class ProviderRegistry : Registry
    {
        public ProviderRegistry()
        {
            For<IIndexSettings<Provider>>().Use<ProviderIndexSettings>();
            For<IGenericIndexerHelper<Provider>>().Use<ProviderHelper>();
            For<IIndexerService<Provider>>().Use<IndexerService<Provider>>();
            For<IIndexMaintenanceService>().Use<IndexMaintenanceService>();
            For<IGetProviders>().Use<ProviderService>();
        }
    }
}