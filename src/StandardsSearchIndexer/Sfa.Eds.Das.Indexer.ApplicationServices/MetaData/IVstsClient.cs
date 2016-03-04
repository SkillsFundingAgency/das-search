namespace Sfa.Eds.Das.Indexer.ApplicationServices.MetaData
{
    public interface IVstsClient
    {
        string GetFileContent(string path);
    }
}