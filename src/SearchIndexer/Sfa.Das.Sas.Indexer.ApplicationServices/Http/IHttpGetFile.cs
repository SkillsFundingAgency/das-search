using System.IO;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Http
{
    public interface IHttpGetFile
    {
        Stream GetFile(string url);
    }
}