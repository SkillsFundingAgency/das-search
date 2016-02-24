using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Sfa.Eds.Das.Indexer.Common.Models;
using Sfa.Eds.Das.Indexer.Common.Settings;

namespace Sfa.Eds.Das.Indexer.Common.Helpers
{
    public class BlobStorageHelper : IBlobStorageHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ICloudBlobClientWrapper _client;

        public BlobStorageHelper(ICloudBlobClientWrapper client)
        {
            _client = client;
        }

        public async Task<List<JsonMetadataObject>> ReadAsync(string containerName)
        {
            var jsonList = new List<JsonMetadataObject>();

            // Retrieve reference to a previously created container.
            var container = _client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            try
            {
                var elements = container.ListBlobs(null, false);
                foreach (var blob in elements.OfType<ICloudBlob>())
                {
                    string text;
                    using (var memoryStream = new MemoryStream())
                    {
                        blob.DownloadToStream(memoryStream);
                        text = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }

                    jsonList.Add(JsonConvert.DeserializeObject<JsonMetadataObject>(text));
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading standards from Azure: " + e.Message);
            }

            return jsonList;
        }

        public async Task<byte[]> ReadStandardPdfAsync(string containerName, string fileName)
        {
            // Retrieve reference to a previously created container.
            var container = _client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

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
                Log.Error("Error uploading standards pdfs to Azure: " + e.Message);
            }
        }
    }
}