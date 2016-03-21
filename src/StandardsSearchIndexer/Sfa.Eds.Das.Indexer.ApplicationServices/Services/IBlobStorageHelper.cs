namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System.Threading.Tasks;

    public interface IBlobStorageHelper
    {
        Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName);
        bool FileExists(string containerName, string fileName);
        Task UploadPdfFromUrl(string containerName, string fileName, string url);
    }
}