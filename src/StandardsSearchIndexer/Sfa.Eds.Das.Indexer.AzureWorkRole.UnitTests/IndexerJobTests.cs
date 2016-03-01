namespace Sfa.Eds.Das.Indexer.AzureWorkRole.UnitTests
{
    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.AzureWorkerRole;
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Common.Services;
    using Sfa.Eds.Das.ProviderIndexer.Models;

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
            mockConsumer.Verify(x => x.CheckMessage<MetaDataItem>());
            mockConsumer.Verify(x => x.CheckMessage<Provider>());
        }
    }
}