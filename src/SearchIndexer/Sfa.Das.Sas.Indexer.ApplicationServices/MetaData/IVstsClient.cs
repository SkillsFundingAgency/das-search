namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    public interface IVstsClient
    {
        string GetFileContent(string path);
    }
}