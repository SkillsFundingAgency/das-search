namespace Sfa.Eds.Das.Indexer.VerticalSliceTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    [TestFixture]
    public class ProviderIndexerVerticalSliceTests
    {
        private IContainer _container;

        private Mock<IGetActiveProviders> _mockActiveProviders;

        private Mock<IGetMessageTimes> _mockCloudQueue;

        private Mock<IMaintainProviderIndex> _mockSearchIndex;

        private Mock<IGetApprenticeshipProviders> _mockCourseDirectoryClient;

        [TestFixtureSetUp]
        public void Setup()
        {
            _container = IoC.Initialize();
            _mockActiveProviders = new Mock<IGetActiveProviders>();
            _mockCloudQueue = new Mock<IGetMessageTimes>();
            _mockSearchIndex = new Mock<IMaintainProviderIndex>();
            _mockCourseDirectoryClient = new Mock<IGetApprenticeshipProviders>();

            _container.Configure(x => x.For<IGetActiveProviders>().Use(_mockActiveProviders.Object));
            _container.Configure(x => x.For<IGetMessageTimes>().Use(_mockCloudQueue.Object));
            _container.Configure(x => x.For<IMaintainProviderIndex>().Use(_mockSearchIndex.Object));
            _container.Configure(x => x.For<IGetApprenticeshipProviders>().Use(_mockCourseDirectoryClient.Object));

            var queue = ConfigurationManager.AppSettings["Provider.QueueName"];

            _mockCloudQueue.Setup(x => x.GetInsertionTimes(queue)).Returns(new List<DateTime> { DateTime.Now });
            _mockSearchIndex.Setup(x => x.IndexExists(It.IsAny<string>())).Returns(true);
            _mockCourseDirectoryClient.Setup(x => x.GetApprenticeshipProvidersAsync()).ReturnsAsync(new List<Provider> {new Provider { Ukprn = 123 }, new Provider { Ukprn = 456 } });
            _mockActiveProviders.Setup(x => x.GetActiveProviders()).Returns(new List<int> { 123 });
            _mockSearchIndex.Setup(x => x.AliasExists(It.IsAny<string>())).Returns(true);
            _mockSearchIndex.Setup(x => x.IndexContainsDocuments(It.IsAny<string>())).Returns(true);

            var sut = _container.GetInstance<IGenericControlQueueConsumer>();

            // Act
            sut.CheckMessage<IMaintainProviderIndex>();
        }

        [Test]
        public void ShouldCheckIfIndexExistsBeforeAndAfterWeCreateTheIndex()
        {
            _mockSearchIndex.Verify(x => x.IndexExists(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void ShouldDeleteIndexIfItExists()
        {
            _mockSearchIndex.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldCreateIndex()
        {
            _mockSearchIndex.Verify(x => x.CreateIndex(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldRetrieveAListOfProvidersFromCourseDirectory()
        {
            _mockCourseDirectoryClient.Verify(x => x.GetApprenticeshipProvidersAsync(), Times.Once);
        }

        [Test]
        public void ShouldRetreiveAListOfActiveProviders()
        {
            _mockActiveProviders.Verify(x => x.GetActiveProviders(), Times.Once);
        }

        [Test]
        public void ShouldIndexOnlyOneProvider()
        {
            _mockSearchIndex.Verify(x => x.IndexEntries(It.IsAny<string>(), It.Is<ICollection<Provider>>(y => y.Count == 1)), Times.Once);
        }

        [Test]
        public void ShouldValidateTheIndexWasCreated()
        {
            _mockSearchIndex.Verify(x => x.IndexContainsDocuments(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldSwapTheIndexAlias()
        {
            _mockSearchIndex.Verify(x => x.AliasExists(It.IsAny<string>()), Times.Once);
            _mockSearchIndex.Verify(x => x.SwapAliasIndex(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShouldDeleteOldIndex()
        {
            _mockSearchIndex.Verify(x => x.DeleteIndexes(It.IsAny<Func<string, bool>>()));
        }
    }
}