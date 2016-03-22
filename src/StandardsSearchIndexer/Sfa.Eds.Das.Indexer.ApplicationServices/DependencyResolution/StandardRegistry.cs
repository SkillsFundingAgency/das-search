namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Standard;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            For<IIndexSettings<IMaintainApprenticeshipIndex>>().Use<StandardIndexSettings>();
            For<IMetaDataHelper>().Use<MetaDataHelper>();
            For<IIndexerService<IMaintainApprenticeshipIndex>>().Use<IndexerService<IMaintainApprenticeshipIndex>>();
            For<IGenericIndexerHelper<IMaintainApprenticeshipIndex>>().Use<ApprenticeshipIndexer>();
        }
    }
}