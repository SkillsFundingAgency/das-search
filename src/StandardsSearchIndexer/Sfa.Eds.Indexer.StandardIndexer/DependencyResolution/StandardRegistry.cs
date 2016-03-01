using Sfa.Eds.Das.StandardIndexer.Helpers;
using Sfa.Eds.Das.StandardIndexer.Settings;
using StructureMap;

namespace Sfa.Eds.Das.StandardIndexer.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.Common.Helpers;
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Common.Services;
    using Sfa.Eds.Das.Indexer.Common.Settings;

    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            For<IStandardHelper>().Use<StandardHelper>();
            For<IStandardIndexSettings>().Use<StandardIndexSettings>();
            For<IMetaDataHelper>().Use<MetaDataHelper>();
            For<IIndexSettings<MetaDataItem>>().Use<StandardIndexSettings>();
            For<IIndexerService<MetaDataItem>>().Use<IndexerService<MetaDataItem>>();
            For<IGenericIndexerHelper<MetaDataItem>>().Use<StandardHelper>();
        }
    }
}