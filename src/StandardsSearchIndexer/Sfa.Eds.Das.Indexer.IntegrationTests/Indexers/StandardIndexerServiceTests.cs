namespace Sfa.Eds.Das.Indexer.IntegrationTests.Indexers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Moq;
    using Nest;
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Standard;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.Elasticsearch;
    using Sfa.Infrastructure.Elasticsearch.Models;
    using Sfa.Infrastructure.Services;

    [TestFixture]
    public class StandardIndexerServiceTests
    {
        private IIndexSettings<MetaDataItem> _standardSettings;

        private IGenericIndexerHelper<MetaDataItem> _indexerService;

        private IElasticClient _elasticClient;

        [SetUp]
        public void SetUp()
        {
            var ioc = IoC.Initialize();
            _standardSettings = ioc.GetInstance<IIndexSettings<MetaDataItem>>();
            _indexerService = ioc.GetInstance<IGenericIndexerHelper<MetaDataItem>>();

            var settings = ioc.GetInstance<IIndexSettings<MetaDataItem>>();

            var maintanSearchIndex = ioc.GetInstance<IMaintanStandardIndex>();

            var moqMetaDataHelper = new Mock<IMetaDataHelper>();
            moqMetaDataHelper.Setup(m => m.UpdateMetadataRepository());
            moqMetaDataHelper.Setup(m => m.GetAllStandardsMetaData()).Returns(GetStandardsTest().ToList());
            moqMetaDataHelper.Setup(m => m.GetAllFrameworkMetaData()).Returns(GetFrameworksTest().ToList());

            var moqLog = new Mock<ILog>();

            _indexerService = new StandardIndexer(settings, maintanSearchIndex, moqMetaDataHelper.Object, moqLog.Object);

            var elasticClientFactory = ioc.GetInstance<IElasticsearchClientFactory>();
            _elasticClient = elasticClientFactory.GetElasticClient();
        }

        [Test]
        [Category("Integration")]
        public void ShouldCreateScheduledIndexAndMappingForStandardsAndFrameworks()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = $"{_standardSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mappingStandards = _elasticClient.GetMapping<StandardDocument>(i => i.Index(indexName));
            mappingStandards.Mappings.Count.Should().NotBe(0);

            var mappingFrameworks = _elasticClient.GetMapping<FrameworkDocument>(i => i.Index(indexName));
            mappingFrameworks.Mappings.Count.Should().NotBe(0);

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldRetrieveStandardSearchingForTitle()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = $"{_standardSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            var expectedStandardResult = new MetaDataItem
                                             {
                                                 Id = 61,
                                                 Title = "Dental Nurse",
                                                 NotionalEndLevel = 3,
                                                 PdfFileName = "61-Apprenticeship standard for a dental nurse",
                                                 StandardPdfUrl = "https://www.gov.uk/government/uploads/system/uploads/attachment_data/file/411720/DENTAL_HEALTH_-_Dental_Nurse.pdf"
                                             };

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            await _indexerService.IndexEntries(indexName);

            Thread.Sleep(2000);

            var retrievedResult = _elasticClient.Search<StandardDocument>(p => p.Index(indexName).QueryString(expectedStandardResult.Title));

            var amountRetrieved = retrievedResult.Documents.Count();
            var retrievedStandard = retrievedResult.Documents.FirstOrDefault();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            Assert.AreEqual(1, amountRetrieved);
            Debug.Assert(retrievedStandard != null, "retrievedStandard != null");
            Assert.AreEqual(expectedStandardResult.Title, retrievedStandard.Title);
            Assert.AreEqual(expectedStandardResult.NotionalEndLevel, retrievedStandard.NotionalEndLevel);
            Assert.AreEqual(expectedStandardResult.Id, retrievedStandard.StandardId);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldRetrieveFrameworksSearchingForAll()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = $"{_standardSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            await _indexerService.IndexEntries(indexName);

            Thread.Sleep(2000);

            //Assert
            var retrievedResultFrameworks = _elasticClient.Search<FrameworkDocument>(p => p.Index(indexName).QueryString("*"));

            var frameworkDocuments = retrievedResultFrameworks.Documents.Count();
            var frameworkDocument = retrievedResultFrameworks.Documents.Single(m => m.FrameworkCode.Equals(423));

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            Assert.AreEqual(3, frameworkDocuments);
            Assert.AreEqual("Fashion and Textiles", frameworkDocument.FrameworkName);
            Assert.AreEqual("Footwear", frameworkDocument.PathwayName);
            Assert.AreEqual(4, frameworkDocument.PathwayCode);
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(i => i.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(i => i.Index(indexName));
            }
        }

        private IEnumerable<MetaDataItem> GetStandardsTest()
        {
            return new List<MetaDataItem>
                       {
                           new MetaDataItem
                               {
                                   Id = 1,
                                   Title = "Network Engineer",
                                   PdfFileName = "1-Apprenticeship standard for a network engineer",
                                   StandardPdfUrl =
                                       "https://www.gov.uk/government/uploads/system/uploads/attachment_data/file/370682/DI_-_Network_engineer_standard.ashx.pdf"
                               },
                           new MetaDataItem
                               {
                                   Id = 2,
                                   Title = "Software Developer",
                                   PdfFileName = "2-Apprenticeship standard for a software developer",
                                   StandardPdfUrl =
                                       "https://www.gov.uk/government/uploads/system/uploads/attachment_data/file/371867/Digital_Industries_-_Software_Developer.pdf"
                               },
                           new MetaDataItem
                               {
                                   Id = 61,
                                   Title = "Dental Nurse",
                                   NotionalEndLevel = 3,
                                   PdfFileName = "61-Apprenticeship standard for a dental nurse",
                                   StandardPdfUrl = "https://www.gov.uk/government/uploads/system/uploads/attachment_data/file/411720/DENTAL_HEALTH_-_Dental_Nurse.pdf"
                               }
                       };
        }

        private IEnumerable<FrameworkMetaData> GetFrameworksTest()
        {
            return new List<FrameworkMetaData>
                       {
                           new FrameworkMetaData
                               {
                                   FworkCode = 403,
                                   ProgType = 0,
                                   PwayCode = 2,
                                   PathwayName = "Baking Industry Skills",
                                   NASTitle = "Food and Drink",
                                   IssuingAuthorityTitle = "Food and Drink - Advanced Level Apprenticeship"
                               },
                           new FrameworkMetaData
                               {
                                   FworkCode = 403,
                                   ProgType = 3,
                                   PwayCode = 7,
                                   PathwayName = "Brewing Industry Skills",
                                   NASTitle = "Food and Drink",
                                   IssuingAuthorityTitle = "Food and Drink - Intermediate Level Apprenticeship"
                               },
                           new FrameworkMetaData
                               {
                                   FworkCode = 423,
                                   ProgType = 2,
                                   PwayCode = 4,
                                   PathwayName = "Footwear",
                                   NASTitle = "Fashion and Textiles",
                                   IssuingAuthorityTitle = "Fashion and Textiles - Advanced Level Apprenticeship"
                               }
                       };
        }
    }
}