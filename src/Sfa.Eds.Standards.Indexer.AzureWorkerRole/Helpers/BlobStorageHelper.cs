using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers
{
    public class BlobStorageHelper
    {
        private readonly CloudStorageAccount _storageAccount;
        private string _accountName = CloudConfigurationManager.GetSetting("StorageAccountName");
        string _key = CloudConfigurationManager.GetSetting("StorageAccountKey");

        public BlobStorageHelper()
        {
            StorageCredentials storageCredentials = new StorageCredentials(_accountName, _key);
            _storageAccount = new CloudStorageAccount(storageCredentials, true);
        }

        public async Task<List<JsonMetadataObject>> ReadStandardsAsync(string containerName)
        {
            List<JsonMetadataObject> standardList = new List<JsonMetadataObject>();

            CloudBlobClient cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            string text;
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    using (var memoryStream = new MemoryStream())
                    {
                        blob.DownloadToStream(memoryStream);
                        text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                    standardList.Add(JsonConvert.DeserializeObject<JsonMetadataObject>(text));
                }
            }

            return standardList;
        }

        public async Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName)
        {
            CloudBlobClient cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named "myblob.txt"
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.FetchAttributes();
            long fileByteLength = blockBlob.Properties.Length;
            byte[] fileContent = new byte[fileByteLength];
            for (int i = 0; i < fileByteLength; i++)
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
                //Creamos el cliente del Blob Storage.
                CloudBlobClient cloudBlobClient = _storageAccount.CreateCloudBlobClient();
                //Crearemos el contenedor del Blob
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
                //Nos aseguraremos de crear el contenedor si no existe. 
                await cloudBlobContainer.CreateIfNotExistsAsync();
                //Obtenemos un bloque del blob
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                //Subimos el archivo a Azure
                using (var fileStream = GenerateStreamFromString(serializedStandard))
                {
                    cloudBlockBlob.UploadFromStream(fileStream);
                }
                //await cloudBlockBlob.UploadFromFileAsync(filePath, FileMode.OpenOrCreate);
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
                //Creamos el cliente del Blob Storage.
                CloudBlobClient cloudBlobClient = _storageAccount.CreateCloudBlobClient();
                //Crearemos el contenedor del Blob
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
                //Nos aseguraremos de crear el contenedor si no existe. 
                await cloudBlobContainer.CreateIfNotExistsAsync();
                //Obtenemos un bloque del blob
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                cloudBlockBlob.StartCopy(new Uri(url), null, null, null);

                bool continueLoop = true;
                while (continueLoop)
                {
                    var blobsList = cloudBlobContainer.ListBlobs(null, true, BlobListingDetails.Copy);
                    foreach (var blob in blobsList)
                    {
                        var tempBlockBlob = (CloudBlob)blob;
                        var destBlob = blob as CloudBlob;

                        if (tempBlockBlob.Name == fileName)
                        {
                            var copyStatus = tempBlockBlob.CopyState;
                            if (copyStatus != null)
                            {
                                if (copyStatus.Status != CopyStatus.Pending)
                                {
                                    continueLoop = false;
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(1000);
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
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}