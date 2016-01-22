using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Helpers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.Test.Services
{
    [TestFixture]
    public class StandardServiceTests
    {
        private Mock<IStandardHelper> _mockHelper;
        private IStandardIndexSettings _mockSettings;
        private StandardService _sut;

        [SetUp]
        public void Setup()
        {
            _mockHelper = new Mock<IStandardHelper>();
            _mockSettings = Mock.Of<IStandardIndexSettings>(x => x.PauseTime == "10");

            _sut = new StandardService(_mockSettings, _mockHelper.Object);
        }

        [Test]
        public void should_not_index_pdfs_if_that_index_already_exists()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(true);
            
            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void should_index_pdfs_if_that_index_does_not_exists_previously()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(false);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.AtMostOnce);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void should_not_swap_idexes_if_new_one_has_not_been_created_properly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(false);
            _mockHelper
                .Setup(x => x.IsIndexCorrectlyCreated())
                .Returns(false);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Never);
            _mockHelper.VerifyAll();
        }

        [Test]
        public void should_swap_idexes_if_new_one_has_been_created_properly()
        {
            // Arrange
            _mockHelper
                .Setup(x => x.CreateIndex(It.IsAny<DateTime>()))
                .Returns(false);
            _mockHelper
                .Setup(x => x.IsIndexCorrectlyCreated())
                .Returns(true);

            // Act
            _sut.CreateScheduledIndex(It.IsAny<DateTime>());

            // Assert
            _mockHelper.Verify(x => x.IndexStandards(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.Verify(x => x.IsIndexCorrectlyCreated(), Times.Once);
            _mockHelper.Verify(x => x.SwapIndexes(It.IsAny<DateTime>()), Times.Once);
            _mockHelper.VerifyAll();
        }
    }
}
