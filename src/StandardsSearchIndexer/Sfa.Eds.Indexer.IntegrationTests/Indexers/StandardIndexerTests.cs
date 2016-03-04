namespace Sfa.Eds.Das.Indexer.IntegrationTests.Indexers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Nest;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;

    using StructureMap;

    [TestFixture]
    public class StandardIndexerTests
    {
        [SetUp]
        public void SetUp()
        {
            _ioc = IoC.Initialize();
            _standardSettings = _ioc.GetInstance<IIndexSettings<MetaDataItem>>();
            _standardHelper = _ioc.GetInstance<IGenericIndexerHelper<MetaDataItem>>();

            var elasticClientFactory = _ioc.GetInstance<IElasticsearchClientFactory>();
            _elasticClient = elasticClientFactory.GetElasticClient();

            _sut = _ioc.GetInstance<IIndexerService<MetaDataItem>>();
        }

        private IContainer _ioc;

        private IIndexSettings<MetaDataItem> _standardSettings;

        private IGenericIndexerHelper<MetaDataItem> _standardHelper;

        private IIndexerService<MetaDataItem> _sut;

        private IElasticClient _elasticClient;

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
                                   StandardPdfUrl =
                                       "https://www.gov.uk/government/uploads/system/uploads/attachment_data/file/411720/DENTAL_HEALTH_-_Dental_Nurse.pdf"
                               }
                       };
        }

        [Test]
        [Category("Integration")]
        public void ShouldCreateScheduledIndexAndMapping()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = _standardHelper.GetIndexNameAndDateExtension(scheduledDate);

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _standardHelper.CreateIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<MetaDataItem>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldRetrieveStandardSearchingForTitle()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = _standardHelper.GetIndexNameAndDateExtension(scheduledDate);

            var standardsTest = GetStandardsTest().ToList();
            var expectedStandardResult = new MetaDataItem
                                             {
                                                 Id = 61,
                                                 Title = "Dental Nurse",
                                                 NotionalEndLevel = 3,
                                                 PdfFileName = "61-Apprenticeship standard for a dental nurse",
                                                 StandardPdfUrl =
                                                     "https://www.gov.uk/government/uploads/system/uploads/attachment_data/file/411720/DENTAL_HEALTH_-_Dental_Nurse.pdf"
                                             };

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            _standardHelper.CreateIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            await _standardHelper.IndexEntries(scheduledDate, standardsTest);

            Thread.Sleep(2000);

            var retrievedResult = _elasticClient.Search<MetaDataItem>(p => p.Index(indexName).QueryString(expectedStandardResult.Title));
            var amountRetrieved = retrievedResult.Documents.Count();
            var retrievedStandard = retrievedResult.Documents.FirstOrDefault();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            Assert.AreEqual(1, amountRetrieved);
            Assert.AreEqual(expectedStandardResult.Title, retrievedStandard.Title);
            Assert.AreEqual(expectedStandardResult.NotionalEndLevel, retrievedStandard.NotionalEndLevel);
            Assert.AreEqual(expectedStandardResult.Id, retrievedStandard.Id);
        }
    }
}