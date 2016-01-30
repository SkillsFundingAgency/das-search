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
        private Mock<IDedsService> _mockDeds;
        private Mock<IBlobStorageHelper> _mockBlob;
        private Mock<IElasticsearchClientFactory> _mockClientFactory;
        private IStandardIndexSettings _mockSettings;

        [SetUp]
        public void Setup()
        {
            _mockDeds = new Mock<IDedsService>();
            _mockBlob = new Mock<IBlobStorageHelper>();
            _mockClientFactory = new Mock<IElasticsearchClientFactory>();
            _mockSettings = Mock.Of<IStandardIndexSettings>();
        }
    }
}
