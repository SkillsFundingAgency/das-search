namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Standard;
    using StructureMap;

    public class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IAppServiceSettings>().Use<AppServiceSettings>();

            // Providers
            For<IIndexSettings<IMaintainProviderIndex>>().Use<ProviderIndexSettings>();
            For<IGenericIndexerHelper<IMaintainProviderIndex>>().Use<ProviderIndexer>();
            For<IIndexerService<IMaintainProviderIndex>>().Use<IndexerService<IMaintainProviderIndex>>();
            For<IProviderFeatures>().Use<ProviderIndexSettings>();

            // Apprenticeships
            For<IIndexSettings<IMaintainApprenticeshipIndex>>().Use<StandardIndexSettings>();
            For<IMetaDataHelper>().Use<MetaDataHelper>();
            For<IIndexerService<IMaintainApprenticeshipIndex>>().Use<IndexerService<IMaintainApprenticeshipIndex>>();
            For<IGenericIndexerHelper<IMaintainApprenticeshipIndex>>().Use<ApprenticeshipIndexer>();
        }
    }
}