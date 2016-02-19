using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.Common.AzureAbstractions;
using Sfa.Eds.Das.Indexer.Settings.Settings;
using Sfa.Eds.Das.StandardIndexer.Consumers;
using Sfa.Eds.Das.StandardIndexer.Services;

namespace Sfa.Eds.Das.StandardIndexer.UnitTests.Consumers
{
    [TestFixture]
    public class StandardControlQueueConsumerTests
    {
        private Mock<IStandardIndexerService> _mockService;
        private Mock<ICloudQueueService> _mockCloudQueueService;
        private Mock<ICloudQueueWrapper> _mockQueue;
        private IStandardIndexSettings _mockSettings;
        private StandardControlQueueConsumer _sut;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _mockService = new Mock<IStandardIndexerService>();
            _mockCloudQueueService = new Mock<ICloudQueueService>();
            _mockQueue = new Mock<ICloudQueueWrapper>();
            _mockSettings = Mock.Of<IStandardIndexSettings>();
            _sut = new StandardControlQueueConsumer(_mockService.Object, _mockSettings, _mockCloudQueueService.Object);
        }

        [Test]
        public void ShouldntCreateAnIndexIfThereArentAnyMessages()
        {
            // Arrange
            _mockCloudQueueService
                .Setup(x => x.GetQueueReference(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockQueue.Object);

            // Act
            var task = _sut.CheckMessage();
            task.Wait();

            // Assert
            _mockService.Verify(x => x.CreateScheduledIndex(It.IsAny<DateTime>()), Times.Never);
            _mockQueue.Verify(x => x.DeleteMessage(It.IsAny<CloudQueueMessage>()), Times.Never);
            _mockService.VerifyAll();
            _mockCloudQueueService.VerifyAll();
            _mockQueue.VerifyAll();
        }

        [Test]
        public void ShouldCreateAnIndexIfThereAreMessages()
        {
            // Arrange
            _mockCloudQueueService
                .Setup(x => x.GetQueueReference(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockQueue.Object);
            _mockQueue
                .Setup(x => x.GetMessages(It.IsAny<int>()))
                .Returns(new List<CloudQueueMessage>() { new CloudQueueMessage(string.Empty) });

            // Act
            var task = _sut.CheckMessage();
            task.Wait();

            // Assert
            _mockService.Verify(x => x.CreateScheduledIndex(It.IsAny<DateTime>()), Times.Once);
            _mockQueue.Verify(x => x.DeleteMessage(It.IsAny<CloudQueueMessage>()), Times.Once);
            _mockService.VerifyAll();
            _mockCloudQueueService.VerifyAll();
            _mockQueue.VerifyAll();
        }
    }
}