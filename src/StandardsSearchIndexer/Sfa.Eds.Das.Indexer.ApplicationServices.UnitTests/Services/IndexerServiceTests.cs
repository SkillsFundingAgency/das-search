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

    [TestFixture]
    public class IndexerServiceTests
    {
        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IGenericIndexerHelper<Core.Models.Provider.Provider>>();
            _mockSettings =
                Mock.Of<IIndexSettings<Core.Models.Provider.Provider>>( // TODO: LWA - This url shouldn't be hardcoded here.
                    x => x.PauseTime == "10" && x.SearchHost == "http://104.45.94.2:9200" && x.IndexesAlias == "ciproviderindexesalias");

            _sut = new IndexerService<Core.Models.Provider.Provider>(_mockSettings, _mockHelper.Object, Mock.Of<ILog>());
        }

        private Mock<IGenericIndexerHelper<Core.Models.Provider.Provider>> _mockHelper;

        private IIndexSettings<Core.Models.Provider.Provider> _mockSettings;

        private IndexerService<Core.Models.Provider.Provider> _sut;

        [Test]
        public async Task ShouldIndexProvidersIfThatIndexHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<DateTime>())).Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<Core.Models.Provider.Provider>>()), Times.Once);
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
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<Core.Models.Provider.Provider>>()), Times.Never);
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
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<Core.Models.Provider.Provider>>()), Times.Once);
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
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<DateTime>(), It.IsAny<ICollection<Core.Models.Provider.Provider>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}