namespace Sfa.Eds.Das.Indexer.AzureWorkRole.UnitTests
{
    using ApplicationServices.Queue;
    using ApplicationServices.Services;
    using AzureWorkerRole;
    using Core.Models;
    using Moq;
    using NUnit.Framework;

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
            mockConsumer.Verify(x => x.CheckMessage<IMaintainStandardIndex>());
            mockConsumer.Verify(x => x.CheckMessage<IMaintainProviderIndex>());
        }
    }
}