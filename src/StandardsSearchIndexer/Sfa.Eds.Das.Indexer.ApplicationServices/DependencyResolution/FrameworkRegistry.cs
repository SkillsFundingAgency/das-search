namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Framework;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    using StructureMap;

    public class FrameworkRegistry : Registry
    {
        public FrameworkRegistry()
        {
            For<IIndexSettings<FrameworkMetaData>>().Use<FrameworkIndexSettings>();
            //For<IMetaDataHelper>().Use<MetaDataHelper>();
            //For<IIndexSettings<MetaDataItem>>().Use<StandardIndexSettings>(); // TBD?
            For<IIndexerService<FrameworkMetaData>>().Use<IndexerService<FrameworkMetaData>>();
            For<IGenericIndexerHelper<FrameworkMetaData>>().Use<FrameworkIndexer>();

        }
    }
}