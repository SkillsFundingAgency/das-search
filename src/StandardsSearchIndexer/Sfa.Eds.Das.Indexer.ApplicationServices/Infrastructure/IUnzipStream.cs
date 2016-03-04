namespace Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure
{
    using System.IO;

    public interface IUnzipStream
    {
        string ExtractFileFromStream(Stream stream, string fileToExtract);
    }
}