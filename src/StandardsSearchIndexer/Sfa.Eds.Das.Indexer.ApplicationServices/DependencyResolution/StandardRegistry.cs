namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Standard;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            For<IIndexSettings<MetaDataItem>>().Use<StandardIndexSettings>();
            For<IMetaDataHelper>().Use<MetaDataHelper>();
            For<IIndexSettings<MetaDataItem>>().Use<StandardIndexSettings>();
            For<IIndexerService<MetaDataItem>>().Use<IndexerService<MetaDataItem>>();
            For<IGenericIndexerHelper<MetaDataItem>>().Use<StandardIndexer>();
        }
    }
}