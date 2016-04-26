using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Das.Sas.Indexer.AzureWorkerRole.Services
{
    public class CloudBlobClientFactory : ICloudBlobClientFactory
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public CloudBlobClientFactory(IWorkerRoleSettings workerRoleSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(workerRoleSettings.StorageConnectionString);

            _cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        public CloudBlobContainer GetContainerReference(string containerName)
        {
            return _cloudBlobClient.GetContainerReference(containerName);
        }
    }
}