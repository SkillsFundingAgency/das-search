namespace Sfa.Das.Sas.Indexer.ApplicationServices.Services
{
    public interface IIndexerServiceFactory
    {
        IIndexerService<T> GetIndexerService<T>();
    }
}
