using Moq;
using NUnit.Framework;
using Sfa.Eds.Indexer.DedsService.Services;
using Sfa.Eds.Indexer.Indexer.Infrastructure.Configuration;
using Sfa.Eds.Indexer.Indexer.Infrastructure.Helpers;
using Sfa.Eds.Indexer.Settings.Settings;

namespace Sfa.Eds.Standards.Indexer.UnitTests.Helpers
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
