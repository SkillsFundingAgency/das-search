namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using Microsoft.WindowsAzure.Storage.Blob;

    public interface ICloudBlobClientFactory
    {
        CloudBlobContainer GetContainerReference(string containerName);
    }
}