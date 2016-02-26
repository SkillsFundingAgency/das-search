using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Eds.Das.Indexer.Common.Helpers
{
    public class CloudBlobContainerWrapper : ICloudBlobContainerWrapper
    {
        private readonly CloudBlobContainer _cloudBlobContainer;

        public CloudBlobContainerWrapper(CloudBlobContainer cloudBlobContainer)
        {
            _cloudBlobContainer = cloudBlobContainer;
        }

        public async Task<bool> CreateIfNotExistsAsync()
        {
            return await _cloudBlobContainer.CreateIfNotExistsAsync();
        }

        public IEnumerable<IListBlobItem> ListBlobs(string prefix = null, bool useFlatBlobListing = false, BlobListingDetails blobListingDetails = BlobListingDetails.None)
        {
            return _cloudBlobContainer.ListBlobs(prefix, useFlatBlobListing, blobListingDetails);
        }

        public CloudBlockBlob GetBlockBlobReference(string blobName)
        {
            return _cloudBlobContainer.GetBlockBlobReference(blobName);
        }
    }
}