namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;

    public class CloudBlobClientFactory : ICloudBlobClientFactory
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public CloudBlobClientFactory(IWorkerRoleSettings workerRoleSettings)
        {
            var storageCredentials = new StorageCredentials(workerRoleSettings.StorageAccountName, workerRoleSettings.StorageAccountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            _cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        public CloudBlobContainer GetContainerReference(string containerName)
        {
            return _cloudBlobClient.GetContainerReference(containerName);
        }
    }
}