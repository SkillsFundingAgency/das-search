using Sfa.Eds.Das.StandardIndexer.Helpers;
using Sfa.Eds.Das.StandardIndexer.Services;
using Sfa.Eds.Das.StandardIndexer.Settings;
using StructureMap;

namespace Sfa.Eds.Das.StandardIndexer.DependencyResolution
{
    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            For<IStandardIndexerService>().Use<StandardIndexerService>();
            For<IStandardHelper>().Use<StandardHelper>();
            For<IStandardIndexSettings>().Use<StandardIndexSettings>();
            For<IMetaDataHelper>().Use<MetaDataHelper>();
        }
    }
}