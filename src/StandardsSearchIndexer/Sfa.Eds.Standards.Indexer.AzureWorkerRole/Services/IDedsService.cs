namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services
{
    public interface IDedsService
    {
        int GetNotationLevelFromLars(int standardId);
    }
}