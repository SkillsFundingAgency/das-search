using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Eds.Das.Indexer.Common.Helpers
{
    public interface ICloudBlobContainerWrapper
    {
        Task<bool> CreateIfNotExistsAsync();
        IEnumerable<IListBlobItem> ListBlobs(string prefix = null, bool useFlatBlobListing = false, BlobListingDetails blobListingDetails = 0);
        CloudBlockBlob GetBlockBlobReference(string blobName);
    }
}