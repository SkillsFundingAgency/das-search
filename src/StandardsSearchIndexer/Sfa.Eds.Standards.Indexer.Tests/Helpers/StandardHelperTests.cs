using Moq;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Configuration;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.Tests.Helpers
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
