namespace Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure
{
    public interface IUnzipFiles
    {
        string ExtractFileFromZip(string pathToZipFile, string fileToExtract);
    }
}
