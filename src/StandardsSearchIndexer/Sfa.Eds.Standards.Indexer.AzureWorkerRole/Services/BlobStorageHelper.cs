namespace Sfa.Eds.Das.Indexer.AzureWorkerRole.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Services;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.Settings;

    public class BlobStorageHelper : IBlobStorageHelper
    {
        private readonly CloudBlobClient _client;

        private readonly ILog _log;


        public BlobStorageHelper(ILog log, IWorkerRoleSettings settings)
        {
            _log = log;

            var storageCredentials = new StorageCredentials(settings.StorageAccountName, settings.StorageAccountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            _client = storageAccount.CreateCloudBlobClient();
        }

        public async Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName)
        {
            // Retrieve reference to a previously created container.
            var container = _client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            var blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.FetchAttributes();
            var fileByteLength = blockBlob.Properties.Length;
            var fileContent = new byte[fileByteLength];

            for (var i = 0; i < fileByteLength; i++)
            {
                fileContent[i] = 0x20;
            }

            await blockBlob.DownloadToByteArrayAsync(fileContent, 0);
            return fileContent;
        }

        public bool FileExists(string containerName, string fileName)
        {
            var container = _client.GetContainerReference(containerName);
            var file = container.GetBlockBlobReference(fileName);

            return file != null;
        }

        public async Task UploadPdfFromUrl(string containerName, string fileName, string url)
        {
            try
            {
                var cloudBlobContainer = _client.GetContainerReference(containerName);

                await cloudBlobContainer.CreateIfNotExistsAsync();

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                cloudBlockBlob.StartCopy(new Uri(url), null, null, null);

                var continueLoop = true;
                while (continueLoop)
                {
                    var blobsList = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Copy);
                    foreach (var blob in blobsList)
                    {
                        var tempBlockBlob = (CloudBlob)blob;

                        if (tempBlockBlob.Name == fileName)
                        {
                            var copyStatus = tempBlockBlob.CopyState;
                            if (copyStatus != null && (copyStatus.Status != CopyStatus.Pending))
                            {
                                continueLoop = false;
                            }
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                _log.Error("Error uploading standards pdfs to Azure: ", e);
            }
        }
    }
}