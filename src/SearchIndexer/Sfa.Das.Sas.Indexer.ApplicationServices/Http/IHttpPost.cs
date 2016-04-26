namespace Sfa.Das.Sas.Indexer.ApplicationServices.Http
{
    public interface IHttpPost
    {
        void Post(string url, string body, string user, string password);
    }
}