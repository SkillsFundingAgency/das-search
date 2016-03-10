namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Infrastructure;
    using ApplicationServices.Services.Interfaces;
    using Settings;
    using Core;
    using Core.Models;

    [TestFixture]
    public class IndexerServiceTests
    {
        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IGenericIndexerHelper<ProviderOld>>();
            _mockSettings =
                Mock.Of<IIndexSettings<ProviderOld>>(
                    x => x.PauseTime == "10" && x.SearchHost == "http://104.45.94.2:9200" && x.IndexesAlias == "ciproviderindexesalias");

            _sut = new IndexerService<ProviderOld>(_mockSettings, _mockHelper.Object, Mock.Of<ILog>());
        }

        private Mock<IGenericIndexerHelper<ProviderOld>> _mockHelper;

        private IIndexSettings<ProviderOld> _mockSettings;

        private IndexerService<ProviderOld> _sut;

        [Test]
        public async Task ShouldIndexProvidersIfThatIndexHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<DateTime>())).Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<ProviderOld>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.AtMostOnce);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldNotIndexProvidersIfThatIndexHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<DateTime>())).Returns(false);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<ProviderOld>>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldNotSwapIdexesIfNewOneHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<DateTime>())).Returns(true);
            _mockHelper.Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>())).Returns(false);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<ProviderOld>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldSwapIdexesIfNewOneHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<DateTime>())).Returns(true);
            _mockHelper.Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>())).Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<ProviderOld>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}