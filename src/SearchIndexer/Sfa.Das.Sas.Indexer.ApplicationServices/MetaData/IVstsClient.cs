namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    public interface IVstsClient
    {
        string GetFileContent(string path);

        string Get(string url);

        void Post(string url, string username, string pwd, string body);

        string GetLatesCommit();
    }
}