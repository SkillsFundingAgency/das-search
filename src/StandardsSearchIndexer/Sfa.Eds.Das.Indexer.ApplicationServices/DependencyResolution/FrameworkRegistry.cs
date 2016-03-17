namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.ApplicationServices.Framework;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    using StructureMap;

    public class FrameworkRegistry : Registry
    {
        public FrameworkRegistry()
        {
            For<IGenericIndexerHelper<FrameworkMetaData>>().Use<FrameworkIndexer>();
        }
    }
}