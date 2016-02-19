using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sfa.Eds.Das.Indexer.Common.Models;

namespace Sfa.Eds.Das.Indexer.Common.Helpers
{
    public interface IBlobStorageHelper
    {
        Task<List<JsonMetadataObject>> ReadStandardsAsync(string containerName);
        Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName);
        Task UploadStandardAsync(string containerName, string fileName, string serializedStandard);
        Task UploadPdfFromUrl(string containerName, string fileName, string url);
        Stream GenerateStreamFromString(string s);
    }
}