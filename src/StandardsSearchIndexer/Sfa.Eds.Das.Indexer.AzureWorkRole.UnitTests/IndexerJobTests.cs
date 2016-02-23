using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.AzureWorkerRole;
using Sfa.Eds.Das.Indexer.Common.Services;
using Sfa.Eds.Das.Indexer.Common.Settings;
using Sfa.Eds.Das.ProviderIndexer.Services;
using Sfa.Eds.Das.StandardIndexer.Services;

namespace Sfa.Eds.Das.Indexer.AzureWorkRole.UnitTests
{
    [TestFixture]
    public class IndexerJobTests
    {
        [Test]
        public void ShouldCheckForProvidersAndStandardsToIndex()
        {
            // Arrange
            var mockConsumer = new Mock<IGenericControlQueueConsumer>();
            var sut = new IndexerJob(mockConsumer.Object);
            // Act

            sut.Run();

            // Assert
            mockConsumer.Verify(x => x.CheckMessage<IStandardIndexerService>());
            mockConsumer.Verify(x => x.CheckMessage<IProviderIndexerService>());
        }
    }
}
