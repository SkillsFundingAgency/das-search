using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public class BlobStorageHelper
    {
        private static readonly StandardIndexSettings StandardIndexSettings = new StandardIndexSettings();

        private readonly string _accountName = StandardIndexSettings.StorageAccountName;
        private readonly string _key = StandardIndexSettings.StorageAccountKey;
        private readonly CloudStorageAccount _storageAccount;

        public BlobStorageHelper()
        {
            var storageCredentials = new StorageCredentials(_accountName, _key);
            _storageAccount = new CloudStorageAccount(storageCredentials, true);
        }

        public List<JsonMetadataObject> ReadStandards(string containerName)
        {
            var standardList = new List<JsonMetadataObject>();

            var cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = cloudBlobClient.GetContainerReference(containerName);

            string text;
            foreach (var item in container.ListBlobs(null, false))
            {
                if (item is CloudBlockBlob)
                {
                    var blob = (CloudBlockBlob) item;

                    using (var memoryStream = new MemoryStream())
                    {
                        blob.DownloadToStream(memoryStream);
                        text = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }

                    standardList.Add(JsonConvert.DeserializeObject<JsonMetadataObject>(text));
                }
            }

            return standardList;
        }

        public async Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName)
        {
            var cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = cloudBlobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named "myblob.txt"
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

        public async Task UploadStandardAsync(string containerName, string fileName, string serializedStandard)
        {
            try
            {
                var cloudBlobClient = _storageAccount.CreateCloudBlobClient();

                var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

                await cloudBlobContainer.CreateIfNotExistsAsync();

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                using (var fileStream = GenerateStreamFromString(serializedStandard))
                {
                    cloudBlockBlob.UploadFromStream(fileStream);
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
                throw;
            }
        }

        public async Task UploadPdfFromUrl(string containerName, string fileName, string url)
        {
            try
            {
                var cloudBlobClient = _storageAccount.CreateCloudBlobClient();

                var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

                await cloudBlobContainer.CreateIfNotExistsAsync();

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                cloudBlockBlob.StartCopy(new Uri(url), null, null, null);

                var continueLoop = true;
                while (continueLoop)
                {
                    var blobsList = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Copy);
                    foreach (var blob in blobsList)
                    {
                        var tempBlockBlob = (CloudBlob) blob;

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
                var error = e.Message;
                throw;
            }
        }

        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}