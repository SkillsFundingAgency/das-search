using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.Common.Models;
using Sfa.Eds.Das.StandardIndexer.Helpers;
using Sfa.Eds.Das.StandardIndexer.Services;
using Sfa.Eds.Das.StandardIndexer.Settings;

namespace Sfa.Eds.Das.StandardIndexer.UnitTests.Services
{
    [TestFixture]
    public class StandardIndexerServiceTests
    {
        private Mock<IStandardHelper> _mockHelper;
        private IStandardIndexSettings _mockSettings;
        private StandardIndexerService _sut;

        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IStandardHelper>();
            _mockSettings = Mock.Of<IStandardIndexSettings>(x => x.PauseTime == "10");

            _sut = new StandardIndexerService(_mockSettings, _mockHelper.Object);
        }

        [Test]
        public void ShouldNotIndexStandardsIfThatIndexHasNotBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>(), It.IsAny<IEnumerable<JsonMetadataObject>>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void ShouldIndexStandardsIfThatIndexHasBeenCreatedProperly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>(), It.IsAny<IEnumerable<JsonMetadataObject>>()), Times.Once);
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
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>(), It.IsAny<IEnumerable<JsonMetadataObject>>()), Times.Once);
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
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>(), It.IsAny<IEnumerable<JsonMetadataObject>>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}
