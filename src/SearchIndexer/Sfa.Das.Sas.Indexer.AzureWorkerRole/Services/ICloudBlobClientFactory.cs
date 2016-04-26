using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole.Services
{
    public interface ICloudBlobClientFactory
    {
        CloudBlobContainer GetContainerReference(string containerName);
    }
}