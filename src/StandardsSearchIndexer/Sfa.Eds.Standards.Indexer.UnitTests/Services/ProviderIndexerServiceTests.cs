using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.Common.Models;
using Sfa.Eds.Das.ProviderIndexer.Helpers;
using Sfa.Eds.Das.ProviderIndexer.Services;
using Sfa.Eds.Das.ProviderIndexer.Settings;

namespace Sfa.Eds.Das.ProviderIndexer.UnitTests.Services
{
    using Sfa.Eds.Das.ProviderIndexer.Clients;

    [TestFixture]
    public class ProviderIndexerServiceTests
    {
        private Mock<IProviderHelper> _mockHelper;
        private IProviderIndexSettings _mockSettings;
        private ProviderIndexerService _sut;
        private Mock<ICourseDirectoryClient> _mockClient;
        private Mock<IActiveProviderClient> _mockActiveProviderClient;

        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IProviderHelper>();
            _mockClient = new Mock<ICourseDirectoryClient>();
            _mockSettings = Mock.Of<IProviderIndexSettings>(x => x.PauseTime == "10" && x.SearchHost == "http://104.45.94.2:9200" && x.ProviderIndexesAlias == "ciproviderindexesalias");
            _mockActiveProviderClient = new Mock<IActiveProviderClient>();

            _sut = new ProviderIndexerService(_mockSettings, _mockHelper.Object, _mockClient.Object, _mockActiveProviderClient.Object);
        }

        [Test]
        public void ShouldNotIndexProvidersIfThatIndexHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void ShouldIndexProvidersIfThatIndexHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.AtMostOnce);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void ShouldNotSwapIdexesIfNewOneHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);
            _mockHelper
                .Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void ShouldSwapIdexesIfNewOneHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);
            _mockHelper
                .Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()))
                .Returns(true);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}
