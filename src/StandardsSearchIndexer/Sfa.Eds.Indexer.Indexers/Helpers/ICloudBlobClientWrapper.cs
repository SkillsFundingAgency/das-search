using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Eds.Das.Indexer.Common.Helpers
{
    public interface ICloudBlobClientWrapper
    {
        ICloudBlobContainerWrapper GetContainerReference(string containerName);
    }
}