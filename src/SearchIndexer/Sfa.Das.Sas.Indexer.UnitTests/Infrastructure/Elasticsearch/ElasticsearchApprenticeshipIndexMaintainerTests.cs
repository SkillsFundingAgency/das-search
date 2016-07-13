using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Nest;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Exceptions;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Elasticsearch
{
    [TestFixture]
    public sealed class ElasticsearchApprenticeshipIndexMaintainerTests : BaseElasticIndexMaintainerTests
    {
        [Test]
        public void ShouldThrowAnExceptionIfCantCreateAnIndex()
        {
            var response = new StubResponse(400);
            MockElasticClient.Setup(x => x.CreateIndex(It.IsAny<IndexName>(), It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), It.IsAny<string>())).Returns(response);

            var indexMaintainer = new ElasticsearchApprenticeshipIndexMaintainer(MockElasticClient.Object, Mock.Of<IElasticsearchMapper>(), Mock.Of<ILog>(), Mock.Of<IElasticsearchConfiguration>());
            Assert.Throws<ConnectionException>(() => indexMaintainer.CreateIndex("testindex"));
        }

        [Test]
        public async Task ShouldBulk2TimeWith4001standards()
        {
            var response = new StubResponse(400);
            var mockResponse = new Mock<IBulkResponse> {DefaultValue = DefaultValue.Mock};
            MockElasticClient.Setup(x => x.CreateIndex(It.IsAny<IndexName>(), It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), It.IsAny<string>())).Returns(response);
            MockElasticClient.Setup(x => x.BulkAsync(It.IsAny<IBulkRequest>(), It.IsAny<string>())).Returns(Task.FromResult<IBulkResponse>(mockResponse.Object));

            var indexMaintainer = new ElasticsearchApprenticeshipIndexMaintainer(
                MockElasticClient.Object, 
                new ElasticsearchMapper(Mock.Of<ILog>()),
                Mock.Of<ILog>(),
                Mock.Of<IElasticsearchConfiguration>());

            await indexMaintainer.IndexStandards("testindex", StandardsTestData4001());

            MockElasticClient.Verify(x => x.BulkAsync(It.IsAny<IBulkRequest>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public async Task ShouldFilterOutStandardsThatAreNotValid()
        {
            var response = new StubResponse(400);
            var mockResponse = new Mock<IBulkResponse> {DefaultValue = DefaultValue.Mock};
            MockElasticClient.Setup(x => x.CreateIndex(It.IsAny<IndexName>(), It.IsAny<Func<CreateIndexDescriptor, ICreateIndexRequest>>(), It.IsAny<string>())).Returns(response);
            MockElasticClient.Setup(x => x.BulkAsync(It.IsAny<IBulkRequest>(), It.IsAny<string>())).Returns(Task.FromResult<IBulkResponse>(mockResponse.Object));

            var indexMaintainer = new ElasticsearchApprenticeshipIndexMaintainer(
                MockElasticClient.Object, 
                new ElasticsearchMapper(Mock.Of<ILog>()), 
                Mock.Of<ILog>(),
                Mock.Of<IElasticsearchConfiguration>());

            await indexMaintainer.IndexStandards("testindex", StandardsTestData4001WithOneNull());

            MockElasticClient.Verify(x => x.BulkAsync(It.IsAny<IBulkRequest>(), It.IsAny<string>()), Times.Exactly(1));
        }

        private static List<StandardMetaData> StandardsTestData4001()
        {
            var standards = new List<StandardMetaData>();
            for (var i = 0; i < 4001; i++)
            {
                standards.Add(new StandardMetaData { Id = 2 });
            }

            return standards;
        }

        private static List<StandardMetaData> StandardsTestData4001WithOneNull()
        {
            var standards = new List<StandardMetaData> {null};
            for (var i = 0; i < 4000; i++)
            {
                standards.Add(new StandardMetaData { Id = 2 });
            }

            return standards;
        }
    }
}