namespace Sfa.Eds.Das.Indexer.Common.AzureAbstractions
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Sfa.Eds.Das.Indexer.Common.Helpers;
    using Sfa.Eds.Das.Indexer.Common.Settings;

    public class CloudBlobClientWrapper : ICloudBlobClientWrapper
    {
        private readonly ICommonSettings _commonSettings;
        private readonly CloudBlobClient _cloudBlobClient;
        public CloudBlobClientWrapper(ICommonSettings commonSettings)
        {
            _commonSettings = commonSettings;

            var storageCredentials = new StorageCredentials(_commonSettings.StorageAccountName, _commonSettings.StorageAccountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            _cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        public ICloudBlobContainerWrapper GetContainerReference(string containerName)
        {
            return new CloudBlobContainerWrapper(_cloudBlobClient.GetContainerReference(containerName));
        }
    }
}