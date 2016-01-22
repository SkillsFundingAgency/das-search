using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.AzureAbstractions;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Consumers;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Services;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings;

namespace Sfa.Eds.Standards.Indexer.Test.Consumers
{
    [TestFixture]
    public class StandardControlQueueConsumerTests
    {
        private Mock<IStandardService> mockService;
        private Mock<ICloudQueueService> mockCloudQueueService;
        private Mock<ICloudQueueWrapper> mockQueue;
        private IStandardIndexSettings mockSettings;
        private StandardControlQueueConsumer sut;

        [SetUp]
        public void setup()
        {
            // Arrange
            mockService = new Mock<IStandardService>();
            mockCloudQueueService = new Mock<ICloudQueueService>();
            mockQueue = new Mock<ICloudQueueWrapper>();
            mockSettings = Mock.Of<IStandardIndexSettings>();
            sut = new StandardControlQueueConsumer(mockService.Object, mockSettings, mockCloudQueueService.Object);
        }

        [Test]
        public void shouldnt_create_an_index_if_there_arent_any_messages()
        {
            //Arrange
            mockCloudQueueService
                .Setup(x => x.GetQueueReference(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mockQueue.Object);
            
            // Act
            sut.CheckMessage();

            // Assert
            mockService.Verify(x => x.CreateScheduledIndex(It.IsAny<DateTime>()), Times.Never);
            mockQueue.Verify(x => x.DeleteMessage(It.IsAny<CloudQueueMessage>()), Times.Never);
            mockService.VerifyAll();
            mockCloudQueueService.VerifyAll();
            mockQueue.VerifyAll();
        }

        [Test]
        public void should_create_an_index_if_there_are_messages()
        {
            // Arrange
            mockCloudQueueService
                .Setup(x => x.GetQueueReference(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mockQueue.Object);
            mockQueue
                .Setup(x => x.GetMessages(It.IsAny<int>()))
                .Returns(new List<CloudQueueMessage>() { new CloudQueueMessage(string.Empty)});

            // Act
            sut.CheckMessage();

            // Assert
            mockService.Verify(x => x.CreateScheduledIndex(It.IsAny<DateTime>()), Times.Once);
            mockQueue.Verify(x => x.DeleteMessage(It.IsAny<CloudQueueMessage>()), Times.Once);
            mockService.VerifyAll();
            mockCloudQueueService.VerifyAll();
            mockQueue.VerifyAll();
        }
    }
}