using Sfa.Das.Sas.Indexer.ApplicationServices.Provider;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.ApplicationServices.Standard;
using Sfa.Das.Sas.Indexer.Core.Services;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.DependencyResolution
{
    public class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IAppServiceSettings>().Use<AppServiceSettings>();
            For<IIndexerServiceFactory>().Use<IndexerServiceFactory>();

            // Providers
            For<IIndexSettings<IMaintainProviderIndex>>().Use<ProviderIndexSettings>();
            For<IGenericIndexerHelper<IMaintainProviderIndex>>().Use<ProviderIndexer>();
            For<IIndexerService<IMaintainProviderIndex>>().Use<IndexerService<IMaintainProviderIndex>>();
            For<IProviderFeatures>().Use<ProviderIndexSettings>();
            For<IProviderDataService>().Use<ProviderDataService>();

            // Apprenticeships
            For<IIndexSettings<IMaintainApprenticeshipIndex>>().Use<StandardIndexSettings>();
            For<IMetaDataHelper>().Use<MetaDataHelper>();
            For<IIndexerService<IMaintainApprenticeshipIndex>>().Use<IndexerService<IMaintainApprenticeshipIndex>>();
            For<IGenericIndexerHelper<IMaintainApprenticeshipIndex>>().Use<ApprenticeshipIndexer>();
        }
    }
}