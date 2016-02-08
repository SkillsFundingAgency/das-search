using System;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.Tests.Services
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
            _mockSettings = Mock.Of<IProviderIndexSettings>(x => x.PauseTime == "10");

            _sut = new ProviderIndexerService(_mockSettings, _mockHelper.Object);
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
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>()), Times.Never);
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
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>()), Times.Once);
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
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>()), Times.Once);
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
            _mockHelper.Verify(x => x.IndexProviders(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}
