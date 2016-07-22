using System.IO;

namespace LARSMetaDataExplorer.Web
{
    public interface IHttpClient
    {
        Stream GetFile(string url);
        string Get(string url, string username, string pwd);
    }
}