using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Queue;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.AzureWorkerRole;

namespace Sfa.Das.Sas.Indexer.AzureWorkRole.UnitTests
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
            mockConsumer.Verify(x => x.CheckMessage<IMaintainApprenticeshipIndex>());
            mockConsumer.Verify(x => x.CheckMessage<IMaintainProviderIndex>());
        }
    }
}