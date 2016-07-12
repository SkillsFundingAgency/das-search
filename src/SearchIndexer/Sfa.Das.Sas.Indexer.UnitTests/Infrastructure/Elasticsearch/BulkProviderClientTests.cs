namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Elasticsearch
{
    using FluentAssertions;

    using Moq;

    using Nest;

    using NUnit.Framework;

    using Sfa.Das.Sas.Indexer.Core.Logging;
    using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch;
    using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

    [TestFixture]
    public class BulkProviderClientTests
    {
        [TestCase(3999, 1)]
        [TestCase(0, 0)]
        [TestCase(4000, 1)]
        [TestCase(4001, 2)]
        [TestCase(16000, 4)]
        public void BatchSizeTest(int provideCount, int tasks)
        {
            var sut = new BulkProviderClient("testindex", Mock.Of<IElasticsearchCustomClient>());

            for (int i = 0; i < provideCount; i++)
            {
                var frameworkProvider = new FrameworkProvider();
                sut.Create<FrameworkProvider>(c => c.Document(frameworkProvider));
            }

            sut.GetTasks().Count.Should().Be(tasks);
        }

        [TestCase(3999, 1)]
        [TestCase(0, 0)]
        [TestCase(4000, 1)]
        [TestCase(4001, 2)]
        [TestCase(16000, 4)]
        public void ShouldCallClient(int provideCount, int callCount)
        {
            var mockElasticCustomClient = new Mock<ElasticsearchCustomClient>(Mock.Of<IElasticsearchClientFactory>(), Mock.Of<ILog>());

            var sut = new BulkProviderClient("testindex", mockElasticCustomClient.Object);

            for (int i = 0; i < provideCount; i++)
            {
                var frameworkProvider = new FrameworkProvider();
                sut.Create<FrameworkProvider>(c => c.Document(frameworkProvider));
            }

            sut.GetTasks();

            mockElasticCustomClient.Verify(x => x.BulkAsync(It.IsAny<IBulkRequest>(), It.IsAny<string>()), Times.Exactly(callCount));
        }
    }
}
