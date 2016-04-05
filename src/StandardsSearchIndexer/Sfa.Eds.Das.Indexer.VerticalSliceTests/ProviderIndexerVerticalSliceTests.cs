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

        private IGetMessageTimes _mockCloudQueue;

        private IMaintainProviderIndex _mockSearchIndex;

        private IGetApprenticeshipProviders _mockCourseDirectoryClient;

        private IClearQueue _mockClearQueue;

        [SetUp]
        public void Setup()
        {
            _container = IoC.Initialize();

            _mockActiveProviders = Substitute.For<IGetActiveProviders>();
            _mockCloudQueue = Substitute.For<IGetMessageTimes>();
            _mockClearQueue = Substitute.For<IClearQueue>();
            _mockSearchIndex = Substitute.For<IMaintainProviderIndex>();
            _mockCourseDirectoryClient = Substitute.For<IGetApprenticeshipProviders>();

            _container.Configure(x => x.For<IGetActiveProviders>().Use(_mockActiveProviders));
            _container.Configure(x => x.For<IGetMessageTimes>().Use(_mockCloudQueue));
            _container.Configure(x => x.For<IMaintainProviderIndex>().Use(_mockSearchIndex));
            _container.Configure(x => x.For<IGetApprenticeshipProviders>().Use(_mockCourseDirectoryClient));
        }

        [Test]
        public void ShouldIndexProviders()
        {
            var queue = ConfigurationManager.AppSettings["Provider.QueueName"];

            _container.Configure(x => x.For<IGetActiveProviders>().Use(_mockActiveProviders));
            _container.Configure(x => x.For<IGetMessageTimes>().Use(_mockCloudQueue));
            _container.Configure(x => x.For<IMaintainProviderIndex>().Use(_mockSearchIndex));
            _container.Configure(x => x.For<IGetApprenticeshipProviders>().Use(_mockCourseDirectoryClient));
            _container.Configure(x => x.For<IClearQueue>().Use(_mockClearQueue));

            _mockCloudQueue.GetInsertionTimes(queue).Returns(new List<DateTime> { DateTime.Now });
            _mockSearchIndex.IndexExists(Arg.Any<string>()).Returns(true);
            _mockCourseDirectoryClient.GetApprenticeshipProvidersAsync().Returns(new List<Provider> { new Provider { Ukprn = 123 }, new Provider { Ukprn = 456 } });
            _mockActiveProviders.GetActiveProviders().Returns(new List<int> { 123 });
            _mockSearchIndex.AliasExists(Arg.Any<string>()).Returns(true);
            _mockSearchIndex.IndexContainsDocuments(Arg.Any<string>()).Returns(true);

            var sut = _container.GetInstance<IGenericControlQueueConsumer>();

            // Act
            sut.CheckMessage<IMaintainProviderIndex>();

            // Assert
            Received.InOrder(() =>
                {
                    _mockCloudQueue.GetInsertionTimes(queue);

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

                    // Clear messages from the queue used to trigger the indexer
                    _mockClearQueue.ClearQueue(queue);
            });

        }
    }
}