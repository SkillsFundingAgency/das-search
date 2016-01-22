using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.Test.Helpers
{
    [TestFixture]
    public class StandardHelperTests
    {
        Mock<IDedsService> _mockDeds;
        Mock<IBlobStorageHelper> _mockBlob;
        Mock<IElasticsearchClientFactory> _mockClient;
        IStandardIndexSettings _mockSettings;

        [SetUp]
        public void Setup()
        {
            _mockDeds = new Mock<IDedsService>();
            _mockBlob = new Mock<IBlobStorageHelper>();
            _mockClient = new Mock<IElasticsearchClientFactory>();
            _mockSettings = Mock.Of<IStandardIndexSettings>();
        }
        [Test]
        public void should_something()
        {
            // Arrange
            StandardHelper sut = new StandardHelper(_mockDeds.Object, _mockBlob.Object, _mockSettings, _mockClient.Object);

            sut.CreateIndex(It.IsAny<DateTime>());
            // Act

            // Assert

        }
    }
}
