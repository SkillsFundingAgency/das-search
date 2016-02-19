using Sfa.Eds.Das.StandardIndexer.Consumers;
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
            For<IStandardControlQueueConsumer>().Use<StandardControlQueueConsumer>();
            For<IStandardIndexSettings>().Use<StandardIndexSettings>();
        }
    }
}