using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Eds.Das.Indexer.Core.Services;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Queue
{
    [TestFixture]
    public sealed class GenericControlQueueConsumerTest
    {
        private const string QueueName = "testQueue";

        private GenericControlQueueConsumer _sut;
        private Mock<IAppServiceSettings> _mockServiceSettings;
        private Mock<IMessageQueueService> _mockQueueService;
        private IContainer _container;
        private Mock<IIndexerService<IMaintainProviderIndex>> _mockIndexerService;
        private Mock<ILog> _mockLogger;
        private Mock<IQueueMessage> _mockMessage; 
        private List<IQueueMessage> _messages;
        
        
        [SetUp]
        public void Setup()
        {
            _mockServiceSettings = new Mock<IAppServiceSettings>();
            _mockQueueService = new Mock<IMessageQueueService>();
            _mockIndexerService = new Mock<IIndexerService<IMaintainProviderIndex>>();
            _mockLogger = new Mock<ILog>();
            _mockMessage = new Mock<IQueueMessage>();
            
            _messages = new List<IQueueMessage>
            {
                _mockMessage.Object
            };

            _container = IoC.Initialize();
            _container.Configure(x => x.For<IIndexerService<IMaintainProviderIndex>>().Use(_mockIndexerService.Object));

            _mockServiceSettings.Setup(x => x.QueueName(typeof(IMaintainProviderIndex))).Returns(QueueName);
            _mockQueueService.Setup(x => x.GetQueueMessageCount(It.IsAny<string>())).Returns(_messages.Count);

            _sut = new GenericControlQueueConsumer(
                _mockServiceSettings.Object,
                _mockQueueService.Object,
                _container,
                _mockLogger.Object);
        }

        [Test]
        public void ShouldRunIndexerIfMessagesArePresent()
        {
            //Assign
            var insertionTime = DateTime.Now;
            _mockMessage.Setup(x => x.InsertionTime).Returns(insertionTime);
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>())).Returns(_messages);
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(insertionTime)).Returns(Task.Factory.StartNew(() => { }));

            //Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            //Assert
            _mockQueueService.Verify(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()),Times.Once);
            _mockIndexerService.Verify(x => x.CreateScheduledIndex(insertionTime), Times.Once());
        }

        [Test]
        public void ShouldNotRunIndexerIfNoMessagesArePresent()
        {
            //Assign
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<IQueueMessage>());
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>()));

            //Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            //Assert
            _mockQueueService.Verify(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _mockIndexerService.Verify(x => x.CreateScheduledIndex(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void ShouldNotRunIndexerIfReturnedMessagesAreNull()
        {
            //Assign
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()));
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>()));

            //Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            //Assert
            _mockQueueService.Verify(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _mockIndexerService.Verify(x => x.CreateScheduledIndex(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void ShouldUseCorrectQueueNameFromSettings()
        {
            //Assign
            _mockMessage.Setup(x => x.InsertionTime).Returns(DateTime.Now);
            _mockQueueService.Setup(x => x.GetQueueMessages(QueueName, It.IsAny<int>())).Returns(_messages);
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>())).Returns(Task.Factory.StartNew(() => { }));

            //Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            //Assert
            _mockQueueService.Verify(x => x.GetQueueMessages(QueueName, It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void ShouldNotGetMessagesIfQueueNameHasNotBeenFound()
        {
            //Assign
            _mockServiceSettings.Setup(x => x.QueueName(typeof(IMaintainProviderIndex)));
            _mockMessage.Setup(x => x.InsertionTime).Returns(DateTime.Now);
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>())).Returns(_messages);
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>())).Returns(Task.Factory.StartNew(() => { }));

            //Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            //Assert
            _mockQueueService.Verify(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void ShouldDeleteAnyExtraMessagesAfterTheLatest()
        {
            // Assign
            var extraMessage = new Mock<IQueueMessage>().Object;
            _messages.Add(extraMessage);
            _mockMessage.Setup(x => x.InsertionTime).Returns(DateTime.Now);
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>())).Returns(_messages);
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>())).Returns(Task.Factory.StartNew(() => { }));
            _mockQueueService.Setup(x => x.DeleteQueueMessages(
                QueueName,
                It.Is<IEnumerable<IQueueMessage>>(
                    m => m.Contains(extraMessage) && 
                    !m.Contains(_mockMessage.Object))));

            // Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            // Assert
            _mockQueueService.Verify(
                x => x.DeleteQueueMessages(
                    QueueName,
                It.Is<IEnumerable<IQueueMessage>>(
                    m => m.Contains(extraMessage) &&
                    !m.Contains(_mockMessage.Object))),
                    Times.Once);
        }

        [Test]
        public void ShouldDeleteFirstMessagesIfIndexingIsSuccessful()
        {
            // Assign
            var extraMessage = new Mock<IQueueMessage>().Object;
            _messages.Add(extraMessage);
            _mockMessage.Setup(x => x.InsertionTime).Returns(DateTime.Now);
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>())).Returns(_messages);
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>())).Returns(Task.Factory.StartNew(() => { }));
            _mockQueueService.Setup(x => x.DeleteQueueMessage(QueueName, _mockMessage.Object));

            // Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            // Assert
            _mockQueueService.Verify(x => x.DeleteQueueMessage(QueueName, _mockMessage.Object));
        }

        [Test]
        public void ShouldGetAllAvailableMessages()
        {
            // Assign
            var messageCount = 100;

            for (var index = 0; index < messageCount; index++)
            {
                _messages.Add(new Mock<IQueueMessage>().Object);
            }

            _mockMessage.Setup(x => x.InsertionTime).Returns(DateTime.Now);
            _mockQueueService.Setup(x => x.GetQueueMessageCount(QueueName)).Returns(_messages.Count);
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), messageCount + 1)).Returns(_messages);
            _mockIndexerService.Setup(x => x.CreateScheduledIndex(It.IsAny<DateTime>())).Returns(Task.Factory.StartNew(() => { }));
            _mockQueueService.Setup(x => x.DeleteQueueMessages(QueueName, It.Is<IEnumerable<IQueueMessage>>(m => m.Count() == messageCount)));

            // Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            // Assert
            _mockQueueService.Verify(x => x.GetQueueMessageCount(QueueName), Times.Once);
            _mockQueueService.Verify(x => x.GetQueueMessages(It.IsAny<string>(), messageCount + 1), Times.Once);
        }

        [Test]
        public void ShouldNotTryToGetMessagesIfThereArentAny()
        {
            // Assign
            _mockQueueService.Setup(x => x.GetQueueMessageCount(It.IsAny<string>())).Returns(0);
            _mockQueueService.Setup(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()));
            
            // Act
            var task = _sut.CheckMessage<IMaintainProviderIndex>();
            task.Wait(1000);

            // Assert
            _mockQueueService.Verify(x => x.GetQueueMessageCount(It.IsAny<string>()), Times.Once);
            _mockQueueService.Verify(x => x.GetQueueMessages(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}
