using System;
using System.Collections.Generic;
using System.Configuration;
using NSubstitute;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Queue;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Standard;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.UnitTests.VerticalSlice
{
    [TestFixture]
    public class ApprenticeshipIndexerVerticalSliceTests
    {
        private readonly string _queue = ConfigurationManager.AppSettings["Apprenticeship.QueueName"];
        private IContainer _container;
        private IMaintainApprenticeshipIndex _mockSearchIndex;
        private IMessageQueueService _mockCloudQueueService;
        private IMetaDataHelper _mockMetaDataHelper;

        [SetUp]
        public void Setup()
        {
            _container = IoC.Initialize();

            _mockCloudQueueService = Substitute.For<IMessageQueueService>();
            _mockSearchIndex = Substitute.For<IMaintainApprenticeshipIndex>();
            _mockMetaDataHelper = Substitute.For<IMetaDataHelper>();

            _container.Configure(x => x.For<IMaintainApprenticeshipIndex>().Use(_mockSearchIndex));
            _container.Configure(x => x.For<IMetaDataHelper>().Use(_mockMetaDataHelper));
            _container.Configure(x => x.For<IMessageQueueService>().Use(_mockCloudQueueService));
        }

        [Test]
        [Ignore("SC lots has changed I don't know how this test was still working")]
        public void ShouldIndexApprenticeships()
        {
            _mockCloudQueueService.GetQueueMessages(_queue).Returns(new List<IQueueMessage>()
            {
                Substitute.For<IQueueMessage>()
            });

            _mockSearchIndex.IndexExists(Arg.Any<string>()).Returns(true);

            _mockMetaDataHelper.GetAllFrameworkMetaData().Returns(new List<FrameworkMetaData> { new FrameworkMetaData() });
            _mockMetaDataHelper.GetAllStandardsMetaData().Returns(new List<StandardMetaData> { new StandardMetaData() });

            _mockSearchIndex.AliasExists(Arg.Any<string>()).Returns(true);
            _mockSearchIndex.IndexContainsDocuments(Arg.Any<string>()).Returns(true);

            var sut = _container.GetInstance<IGenericControlQueueConsumer>();

            // Act
            sut.CheckMessage<IMaintainApprenticeshipIndex>();

            // Assert
            Received.InOrder(
                () =>
                    {
                        // Check for a trigger message on the queue
                        _mockCloudQueueService.GetQueueMessages(_queue);

                        // If index exists, delete and recreate it
                        _mockSearchIndex.IndexExists(Arg.Any<string>());
                        _mockSearchIndex.DeleteIndex(Arg.Any<string>());
                        _mockSearchIndex.CreateIndex(Arg.Any<string>());
                        _mockSearchIndex.IndexExists(Arg.Any<string>());

                        // Create meta data for new standards
                        _mockMetaDataHelper.UpdateMetadataRepository();

                        // Index standards
                        _mockMetaDataHelper.GetAllStandardsMetaData();
                        _mockSearchIndex.IndexStandards(Arg.Any<string>(), Arg.Any<ICollection<StandardMetaData>>());

                        // Index frameworks
                        _mockMetaDataHelper.GetAllFrameworkMetaData();
                        _mockSearchIndex.IndexFrameworks(Arg.Any<string>(), Arg.Any<ICollection<FrameworkMetaData>>());

                        // Swap the alias with the new index name
                        _mockSearchIndex.IndexContainsDocuments(Arg.Any<string>());
                        _mockSearchIndex.AliasExists(Arg.Any<string>());
                        _mockSearchIndex.SwapAliasIndex(Arg.Any<string>(), Arg.Any<string>());

                        // Delete the old indices
                        _mockSearchIndex.DeleteIndexes(Arg.Any<Func<string, bool>>());
                    });
        }
    }
}