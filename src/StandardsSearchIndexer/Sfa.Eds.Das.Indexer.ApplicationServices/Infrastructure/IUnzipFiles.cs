namespace Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure
{
    public interface IUnzipFiles
    {
        string ExtractFileFromZip(string pathToZipFile, string fileToExtract);
    }
}
