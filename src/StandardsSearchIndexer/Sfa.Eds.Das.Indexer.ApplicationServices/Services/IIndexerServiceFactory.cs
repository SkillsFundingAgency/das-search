namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    public interface IIndexerServiceFactory
    {
        IIndexerService<T> GetIndexerService<T>();
    }
}
