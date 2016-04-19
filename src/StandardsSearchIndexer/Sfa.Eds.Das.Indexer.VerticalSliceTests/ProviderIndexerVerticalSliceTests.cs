namespace Sfa.Eds.Das.Indexer.VerticalSliceTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using NSubstitute;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Queue;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;

    using StructureMap;

    [TestFixture]
    public class ProviderIndexerVerticalSliceTests
    {
        private IContainer _container;
        private IGetActiveProviders _mockActiveProviders;
        private IMaintainProviderIndex _mockSearchIndex;
        private IGetApprenticeshipProviders _mockCourseDirectoryClient;
        private IMessageQueueService _mockCloudQueueService;

        private readonly string _queue = ConfigurationManager.AppSettings["Provider.QueueName"];

        [SetUp]
        public void Setup()
        {
            _container = IoC.Initialize();

            _mockActiveProviders = Substitute.For<IGetActiveProviders>();
           
            _mockCloudQueueService = Substitute.For<IMessageQueueService>();
            _mockSearchIndex = Substitute.For<IMaintainProviderIndex>();
            _mockCourseDirectoryClient = Substitute.For<IGetApprenticeshipProviders>();

            _container.Configure(x => x.For<IGetActiveProviders>().Use(_mockActiveProviders));
            _container.Configure(x => x.For<IMaintainProviderIndex>().Use(_mockSearchIndex));
            _container.Configure(x => x.For<IGetApprenticeshipProviders>().Use(_mockCourseDirectoryClient));
            _container.Configure(x => x.For<IMessageQueueService>().Use(_mockCloudQueueService));
        }

        [Test]
        public void ShouldIndexProviders()
        {
            _mockCloudQueueService.GetQueueMessages(_queue).Returns(new List<IQueueMessage>()
            {
                Substitute.For<IQueueMessage>()
            });

            _mockSearchIndex.IndexExists(Arg.Any<string>()).Returns(true);

            _mockCourseDirectoryClient.GetApprenticeshipProvidersAsync().Returns(new List<Provider> { new Provider { Ukprn = 123 }, new Provider { Ukprn = 456 } });
            _mockActiveProviders.GetActiveProviders().Returns(new List<int> { 123 });
            _mockSearchIndex.AliasExists(Arg.Any<string>()).Returns(true);
            _mockSearchIndex.IndexContainsDocuments(Arg.Any<string>()).Returns(true);

            var sut = _container.GetInstance<IGenericControlQueueConsumer>();

            // Act
            sut.CheckMessage<IMaintainProviderIndex>();

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

                        // Load providers
                        _mockCourseDirectoryClient.GetApprenticeshipProvidersAsync();
                        _mockActiveProviders.GetActiveProviders();

                        // Index the providers
                        _mockSearchIndex.IndexEntries(Arg.Any<string>(), Arg.Any<ICollection<Provider>>());
                        _mockSearchIndex.IndexContainsDocuments(Arg.Any<string>());

                        // Swap the alias with the new index name
                        _mockSearchIndex.AliasExists(Arg.Any<string>());
                        _mockSearchIndex.SwapAliasIndex(Arg.Any<string>(), Arg.Any<string>());

                        // Delete the old indices
                        _mockSearchIndex.DeleteIndexes(Arg.Any<Func<string, bool>>());
                    });
        }
    }
}