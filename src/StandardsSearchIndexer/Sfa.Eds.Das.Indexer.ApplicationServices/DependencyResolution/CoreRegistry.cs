namespace Sfa.Eds.Das.Indexer.ApplicationServices.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IGetProviders>().Use<ProviderService>();
        }
    }
}