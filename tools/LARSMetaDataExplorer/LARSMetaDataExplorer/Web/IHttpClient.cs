using System.IO;

namespace LARSMetaDataToolBox.Web
{
    public interface IHttpClient
    {
        Stream GetFile(string url);
        string Get(string url, string username, string pwd);
    }
}