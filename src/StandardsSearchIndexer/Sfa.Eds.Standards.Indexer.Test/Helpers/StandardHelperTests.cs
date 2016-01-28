using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Moq;
using Nest;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.Test.Helpers
{
    [TestFixture]
    public class StandardHelperTests
    {
        Mock<IDedsService> _mockDeds;
        Mock<IBlobStorageHelper> _mockBlob;
        Mock<IElasticsearchClientFactory> _mockClientFactory;
        IStandardIndexSettings _mockSettings;

        [SetUp]
        public void Setup()
        {
            _mockDeds = new Mock<IDedsService>();
            _mockBlob = new Mock<IBlobStorageHelper>();
            _mockClientFactory = new Mock<IElasticsearchClientFactory>();
            _mockSettings = Mock.Of<IStandardIndexSettings>();
        }

        /*
        [Test]
        [Ignore]
        public void should_something()
        {
            // Arrange
            var mockClient = new Mock<IElasticClient>();
            StandardHelper sut = new StandardHelper(_mockDeds.Object, _mockBlob.Object, _mockSettings, _mockClientFactory.Object);
            _mockClientFactory.Setup(x => x.GetElasticClient()).Returns(mockClient.Object);
            mockClient.Setup(x => x.IndexExists(It.IsAny<string>())).Returns(Mock.Of<IExistsResponse>(x => x.Exists));
            mockClient.Setup(x => x.Count<StandardDocument>(It.IsAny<ICountRequest>())).Returns(Mock.Of<ICountResponse>(x => x.Count == 7));

            // Act
            sut.CreateIndex(It.IsAny<DateTime>());
            // Assert

        }*/
    }
}
