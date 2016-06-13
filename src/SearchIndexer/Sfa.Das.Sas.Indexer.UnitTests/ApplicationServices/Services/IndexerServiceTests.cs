using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.UnitTests.ApplicationServices.Services
{
    [TestFixture]
    public class IndexerServiceTests
    {
        private Mock<IGenericIndexerHelper<Indexer.Core.Models.Provider.Provider>> _mockHelper;
        private IIndexSettings<Indexer.Core.Models.Provider.Provider> _mockSettings;
        private IndexerService<Indexer.Core.Models.Provider.Provider> _sut;

        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IGenericIndexerHelper<Indexer.Core.Models.Provider.Provider>>();
            _mockSettings =
                Mock.Of<IIndexSettings<Indexer.Core.Models.Provider.Provider>>(
                    x => x.PauseTime == "10" && x.IndexesAlias == "testproviderindexesalias");

            _sut = new IndexerService<Indexer.Core.Models.Provider.Provider>(_mockSettings, _mockHelper.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task ShouldIndexProvidersIfThatIndexHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<string>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<string>()), Times.Once);
            _mockHelper.Verify(x => x.ChangeUnderlyingIndexForAlias(It.IsAny<string>()), Times.AtMostOnce);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void ShouldNotIndexProvidersIfThatIndexHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(false);

            // Act
            Assert.Throws<Exception>(async () => await _sut.CreateScheduledIndex(It.IsAny<DateTime>()));

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<string>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldNotSwapIdexesIfNewOneHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(true);
            _mockHelper.Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<string>())).Returns(false);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<string>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<string>()), Times.Once);
            _mockHelper.Verify(x => x.ChangeUnderlyingIndexForAlias(It.IsAny<string>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public async Task ShouldSwapIdexesIfNewOneHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper.Setup(x => x.CreateIndex(It.IsAny<string>())).Returns(true);
            _mockHelper.Setup(x => x.IsIndexCorrectlyCreated(It.IsAny<string>())).Returns(true);

            // Act
            await _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexEntries(It.IsAny<string>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<string>()), Times.Once);
            _mockHelper.Verify(x => x.ChangeUnderlyingIndexForAlias(It.IsAny<string>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}