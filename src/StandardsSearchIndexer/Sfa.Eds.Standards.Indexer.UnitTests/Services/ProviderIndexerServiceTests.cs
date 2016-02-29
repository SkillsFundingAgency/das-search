using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.Common.Models;
using Sfa.Eds.Das.ProviderIndexer.Helpers;
using Sfa.Eds.Das.ProviderIndexer.Services;
using Sfa.Eds.Das.ProviderIndexer.Settings;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.ProviderIndexer.UnitTests.Services
{
    [TestFixture]
    public class ProviderIndexerServiceTests
    {
        private Mock<IProviderHelper> _mockHelper;
        private IProviderIndexSettings _mockSettings;
        private ProviderIndexerService _sut;

        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IProviderHelper>();
            _mockSettings = Mock.Of<IProviderIndexSettings>(x => x.PauseTime == "10" && x.SearchHost == "http://104.45.94.2:9200" && x.ProviderIndexesAlias == "ciproviderindexesalias");

            _sut = new ProviderIndexerService(_mockSettings, _mockHelper.Object);
        }

        [Test]
        public async Task ShouldNotIndexProvidersIfThatIndexHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldIndexProvidersIfThatIndexHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.AtMostOnce);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldNotSwapIdexesIfNewOneHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);
            _mockHelper
                .Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldSwapIdexesIfNewOneHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);
            _mockHelper
                .Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()))
                .Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>(), It.IsAny<List<Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}
