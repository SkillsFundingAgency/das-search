namespace Sfa.Eds.Das.Indexer.IntegrationTests.Indexers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Nest;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Infrastructure.Elasticsearch;
    using Sfa.Infrastructure.Services;

    [TestFixture]
    public class FrameworkIndexerServiceTests
    {
        private IIndexSettings<FrameworkMetaData> _standardSettings;

        private IGenericIndexerHelper<FrameworkMetaData> _indexerService;

        private IElasticClient _elasticClient;

        [SetUp]
        public void SetUp()
        {
            var ioc = IoC.Initialize();
            _standardSettings = ioc.GetInstance<IIndexSettings<FrameworkMetaData>>();
            _indexerService = ioc.GetInstance<IGenericIndexerHelper<FrameworkMetaData>>();

            var elasticClientFactory = ioc.GetInstance<IElasticsearchClientFactory>();
            _elasticClient = elasticClientFactory.GetElasticClient();
        }

        [Test]
        [Category("Integration")]
        public void ShouldCreateScheduledIndexAndMappingForFrameworks()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName =
                $"{_standardSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<FrameworkMetaData>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldRetrieveStandardSearchingForTitle()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName =
                $"{_standardSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            var frameworksTest = GetStandardsTest().ToList();

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            await _indexerService.IndexEntries(indexName);

            Thread.Sleep(2000);

            var retrievedResult = _elasticClient.Search<FrameworkDocument>(p => p.Index(indexName).QueryString("Baking"));
            var amountRetrieved = retrievedResult.Documents.Count();
            var retrievedFramework = retrievedResult.Documents.FirstOrDefault();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            Assert.AreEqual(1, amountRetrieved);
            Assert.AreEqual("Food and Drink - Advanced Level Apprenticeship: Baking Industry Skills", retrievedFramework.Title);
            Assert.AreEqual("2", retrievedFramework.PathwayCode);
            Assert.AreEqual("Food and Drink - Advanced Level Apprenticeship", retrievedFramework.FrameworkName);
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(i => i.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(i => i.Index(indexName));
            }
        }

        private IEnumerable<FrameworkMetaData> GetStandardsTest()
        {
            return new List<FrameworkMetaData>
                       {
                           new FrameworkMetaData
                               {
                                   FworkCode = "403",
                                   ProgType = "0",
                                   PwayCode = "2",
                                   PathwayName = "Baking Industry Skills",
                                   NASTitle = "Food and Drink",
                                   IssuingAuthorityTitle = "Food and Drink - Advanced Level Apprenticeship"
                               },
                           new FrameworkMetaData
                               {
                                   FworkCode = "403",
                                   ProgType = "3",
                                   PwayCode = "7",
                                   PathwayName = "Brewing Industry Skills",
                                   NASTitle = "Food and Drink",
                                   IssuingAuthorityTitle = "Food and Drink - Intermediate Level Apprenticeship"
                               },
                           new FrameworkMetaData
                               {
                                   FworkCode = "423",
                                   ProgType = "2",
                                   PwayCode = "4",
                                   PathwayName = "Footwear",
                                   NASTitle = "Fashion and Textiles",
                                   IssuingAuthorityTitle = "Fashion and Textiles - Advanced Level Apprenticeship"
                               }
                       };
        }
    }
}