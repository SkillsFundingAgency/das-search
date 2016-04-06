using System;
using Moq;
using Nest;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.Core.Exceptions;
using Sfa.Eds.Das.Indexer.Core.Services;
using Sfa.Infrastructure.Elasticsearch;

namespace Sfa.Infrastructure.UnitTests.Elasticsearch
{
    [TestFixture]
    public sealed class ElasticsearchApprenticeshipIndexMaintainerTests : BaseElasticIndexMaintainerTests
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        [ExpectedException(typeof(ConnectionException))]
        public void ShouldThrowAnExceptionIfCantCreateAnIndex()
        {
            var response = new StubResponse(400);
            _mockElasticClient.Setup(x => x.CreateIndex(It.IsAny<IndexName>(), It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>())).Returns(response);

            var indexMaintainer = new ElasticsearchApprenticeshipIndexMaintainer(_mockElasticClientFactory.Object, Mock.Of<IElasticsearchMapper>(), Mock.Of<ILog>());

            indexMaintainer.CreateIndex("testindex");
        }
    }
}
