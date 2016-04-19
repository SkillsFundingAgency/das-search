using StructureMap;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    public class IndexerServiceFactory : IIndexerServiceFactory
    {
        private readonly IContainer _container;

        public IndexerServiceFactory(IContainer container)
        {
            _container = container;
        }

        public IIndexerService<T> GetIndexerService<T>()
        {
            return _container.GetInstance<IIndexerService<T>>();
        }
    }
}
