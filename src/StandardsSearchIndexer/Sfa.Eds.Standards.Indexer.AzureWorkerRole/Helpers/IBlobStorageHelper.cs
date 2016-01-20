using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public interface IBlobStorageHelper
    {
        List<JsonMetadataObject> ReadStandards(string containerName);
        Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName);
        Task UploadStandardAsync(string containerName, string fileName, string serializedStandard);
        Task UploadPdfFromUrl(string containerName, string fileName, string url);
        Stream GenerateStreamFromString(string s);
    }
}