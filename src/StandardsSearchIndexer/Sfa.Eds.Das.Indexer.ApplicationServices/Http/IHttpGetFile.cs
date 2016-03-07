namespace Sfa.Eds.Das.Indexer.ApplicationServices.Http
{
    using System.IO;

    public interface IHttpGetFile
    {
        Stream GetFile(string url);
    }
}