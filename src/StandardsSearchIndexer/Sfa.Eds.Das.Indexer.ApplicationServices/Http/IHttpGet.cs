namespace Sfa.Eds.Das.Indexer.ApplicationServices.Http
{
    public interface IHttpGet
    {
        string Get(string url, string username, string pwd);
    }
}