using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.Common.Helpers;
using Sfa.Eds.Das.Indexer.Common.Models;

namespace Sfa.Eds.Das.Indexer.Common.UnitTests
{
    [TestFixture]
    public class BlobStorageHelperTests
    {
        [Test]
        public async Task ShouldReturnJsonListFromContainerIfThereIsSomethingIntoIt()
        {
            // Arrange
            var mockClient = new Mock<ICloudBlobClientWrapper>();
            var mockContainer = new Mock<ICloudBlobContainerWrapper>();
            var sut = new BlobStorageHelper(mockClient.Object);
            var item = new Mock<ICloudBlob>();
            var blobListWithOneElement = new List<ICloudBlob>()
            {
                item.Object
            //new CloudBlockBlob(new Uri("https://dascistorage.blob.core.windows.net/standardsjson/1"))
            };
            mockContainer.Setup(x => x.ListBlobs(null, false, It.IsAny<BlobListingDetails>())).Returns(blobListWithOneElement);
            
            mockClient.Setup(x => x.GetContainerReference(It.IsAny<string>())).Returns(mockContainer.Object);
            // Act
            var result = await sut.ReadAsync(It.IsAny<string>());

            // Assert
            var expectedList = new List<MetaDataItem>
            {
                null
            };
            result.Should().NotBeNull();
            result.Should().Equals(expectedList);
            item.Verify(x => x.DownloadToStream(It.IsAny<Stream>(), null, null, null));
            mockClient.Verify(x => x.GetContainerReference(It.IsAny<string>()));
        }
    }
}
