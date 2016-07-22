using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Provider;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.IntegrationTests.Indexers
{
    using System.Collections.Generic;

    using Sfa.Das.Sas.Indexer.Core.Models;

    using Address = Sfa.Das.Sas.Indexer.Core.Models.Provider.Address;

    [TestFixture]
    public class ProviderIndexerServiceTest
    {
        private IContainer _ioc;

        private IGenericIndexerHelper<IMaintainProviderIndex> _providerIndexerHelper;

        private IElasticsearchCustomClient _elasticClient;

        private IIndexSettings<IMaintainProviderIndex> _providerSettings;

        private string _indexName;

        [OneTimeSetUp]
        public void SetUp()
        {
            _ioc = IoC.Initialize();

            _providerIndexerHelper = _ioc.GetInstance<IGenericIndexerHelper<IMaintainProviderIndex>>();
            _providerSettings = _ioc.GetInstance<IIndexSettings<IMaintainProviderIndex>>();
            var maintainSearchIndexer = _ioc.GetInstance<IMaintainProviderIndex>();

            var providerDataService = new Mock<IProviderDataService>();
            providerDataService.Setup(m => m.GetProviders()).Returns(GetProvidersTest());
            var logger = new Mock<ILog>();

            _providerIndexerHelper = new ProviderIndexer(_providerSettings, maintainSearchIndexer, providerDataService.Object, logger.Object);

            _elasticClient = _ioc.GetInstance<IElasticsearchCustomClient>();

            _indexName = $"{_providerSettings.IndexesAlias}-{new DateTime(2000, 1, 1).ToUniversalTime().ToString("yyyy-MM-dd-HH-mm")}".ToLower(CultureInfo.InvariantCulture);

            DeleteIndexIfExists(_indexName);

            if (!_elasticClient.IndexExists(Indices.Index(_indexName)).Exists)
            {
                _providerIndexerHelper.CreateIndex(_indexName);
                var indexTask = _providerIndexerHelper.IndexEntries(_indexName);
                Task.WaitAll(indexTask);
                Thread.Sleep(1000);
            }
        }

        [OneTimeTearDown]
        public void AfterAllTestAreRun()
        {
            _elasticClient.DeleteIndex(Indices.Index(_indexName));
            _elasticClient.IndexExists(Indices.Index(_indexName)).Exists.Should().BeFalse();
        }

        [Test]
        [Category("Integration")]
        public void ShouldCreateIndexCorrectly()
        {
            _providerIndexerHelper.IsIndexCorrectlyCreated(_indexName).Should().BeTrue();
        }

        [Test]
        [Category("Integration")]
        public void ShouldCreateScheduledIndexAndMappingForProviders()
        {
            var mappingFramework = _elasticClient.GetMapping<FrameworkProvider>(
                i => i.Index(_indexName));
            mappingFramework.Mappings.Count.Should().Be(1);

            var mappingStandard = _elasticClient.GetMapping<StandardProvider>(i => i.Index(_indexName));
            mappingStandard.Mappings.Count.Should().Be(1);

            var mappingDummy = _elasticClient.GetMapping<DummyTestClass>(i => i.Index(_indexName));
            mappingDummy.Mappings.Count.Should().Be(0);
        }

        [Test]
        [Category("Integration")]
        public void ShouldRetrieveProvidersSearchingForPostCode()
        {
            var expectedProviderResult = new Provider
                                             {
                                                 Ukprn = 10002387,
                                                 Name = "F1 COMPUTER SERVICES & TRAINING LIMITED",
                                                 MarketingInfo = "Provider Marketing Information for F1 COMPUTER SERVICES & TRAINING LIMITED",
                                                 ContactDetails = new ContactInformation { Email = "test1@example.com", Website = "http://www.f1training.org.uk", Phone = "01449 770911" }
                                             };

            var retrievedResult = _elasticClient.Search<FrameworkProvider>(p => p.Index(_indexName).Query(q => q.QueryString(qs => qs.Query("MK40 2SG"))));
            var amountRetrieved = retrievedResult.Documents.Count();
            var retrievedProvider = retrievedResult.Documents.FirstOrDefault();

            Assert.AreEqual(1, amountRetrieved);
            Assert.AreEqual(expectedProviderResult.Name, retrievedProvider?.ProviderName);
        }

        [Test]
        [Category("Integration")]
        public void ShouldNotIndexProvidersWithoutGeoPoint()
        {
            var providersCaseFramework = _elasticClient.Search<Provider>(s => s
               .Index(_indexName)
               .Type(_providerSettings.FrameworkProviderDocumentType)
               .Query(q => q
                   .Term("frameworkCode", 45)));

            var providersCaseStandard = _elasticClient.Search<Provider>(s => s
               .Index(_indexName)
               .Type(_providerSettings.StandardProviderDocumentType)
               .Query(q => q
                   .Term("standardCode", 45)));

            providersCaseFramework.Hits.Count().Should().Be(1);
            providersCaseStandard.Hits.Count().Should().Be(1);
        }

        [Test]
        [Category("Integration")]
        public void ShouldRetrieveProvidersSearchingForStandardId()
        {
            var providersCase1 = _elasticClient.Search<Provider>(s => s
                .Index(_indexName)
                .Type(_providerSettings.StandardProviderDocumentType)
                .Query(q => q
                    .Term("standardCode", 17)));

            var providersCase2 = _elasticClient.Search<Provider>(s => s
                .Index(_indexName)
                .Type(_providerSettings.StandardProviderDocumentType)
                .Query(q => q
                    .Term("standardCode", 45)));

            var providersCase3 = _elasticClient.Search<Provider>(s => s
                .Index(_indexName)
                .Type(_providerSettings.StandardProviderDocumentType)
                .Query(q => q
                    .Term("standardCode", 1234567890)));

            var providersCase4 = _elasticClient.Search<Provider>(s => s
                .Index(_indexName)
                .Type(_providerSettings.FrameworkProviderDocumentType)
                .Query(q => q
                    .Term("frameworkCode", 45)));

            Assert.AreEqual(1, providersCase1.Documents.Count());
            Assert.AreEqual(1, providersCase2.Documents.Count());
            Assert.AreEqual(0, providersCase3.Documents.Count());
            Assert.AreEqual(1, providersCase4.Documents.Count());

            _elasticClient.DeleteIndex(Indices.Index(_indexName));
            _elasticClient.IndexExists(Indices.Index(_indexName)).Exists.Should().BeFalse();
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(Indices.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(Indices.Index(indexName));
            }
        }

        internal class DummyTestClass
        {
        }

        private async Task<ICollection<Provider>> GetProvidersTest()
        {
            var providerLocations = new List<Location>
                                        {
                                            new Location()
                                                {
                                                    Id = 115640,
                                                    Name = "Null GeoPoint",
                                                    Address =
                                                        new Address
                                                        {
                                                            Address1 = "Enterprise House 2",
                                                            Address2 = "2-6 Union Street",
                                                            Town = "Bedford",
                                                            County = null,
                                                            Postcode = "MK40 2SG",
                                                            GeoPoint = null
                                                        },
                                                    Contact = new ContactInformation { Website = "http://testsite.com", Email = "test@test.com", Phone = "0111222222" }
                                                },
                                            new Location
                                                {
                                                    Id = 115641,
                                                    Name = "F1 TRAINING LTD - BEDFORD LEARNING CENTRE",
                                                    Address =
                                                        new Address
                                                            {
                                                                Address1 = "Enterprise House",
                                                                Address2 = "2-6 Union Street",
                                                                Town = "Bedford",
                                                                County = null,
                                                                Postcode = "MK40 2SG",
                                                                GeoPoint = new Coordinate { Latitude = 52.139922, Longitude = -0.475378 }
                                                            },
                                                    Contact = new ContactInformation { Website = "http://testsite.com", Email = "test@test.com", Phone = "0111222222" }
                                                },
                                            new Location
                                                {
                                                    Id = 115643,
                                                    Name = "F1 TRAINING LTD - GT YARMOUTH LEARNING CENTRE",
                                                    Address =
                                                        new Address
                                                            {
                                                                Address1 = "Catalyst - Business Acceleration Centre",
                                                                Address2 = "The Conge",
                                                                Town = "Great Yarmouth",
                                                                County = null,
                                                                Postcode = "NR30 1NA",
                                                                GeoPoint = new Coordinate { Latitude = 52.609776, Longitude = 1.725685 }
                                                            },
                                                    Contact = new ContactInformation { Website = "http://testsite2.com", Email = "test2@test.com", Phone = "033444555" }
                                                }
                                        };

            return new List<Provider>
                       {
                           new Provider
                               {
                                   Id = "304107",
                                   Ukprn = 10002387,
                                   Name = "F1 COMPUTER SERVICES & TRAINING LIMITED",
                                   MarketingInfo = "Provider Marketing Information for F1 COMPUTER SERVICES & TRAINING LIMITED",
                                   ContactDetails = new ContactInformation { Email = "test1@example.com", Website = "http://www.f1training.org.uk", Phone = "01449 770911" },
                                   LearnerSatisfaction = null,
                                   EmployerSatisfaction = null,
                                   Standards =
                                       new List<StandardInformation>
                                           {
                                               new StandardInformation
                                                   {
                                                       Code = 17,
                                                       MarketingInfo = "Provider 304107 marketing into for standard code 17",
                                                       InfoUrl = "www.Provider304107Standard17StandardInfoURL.com",
                                                       ContactInformation =
                                                           new ContactInformation
                                                               {
                                                                   Phone = "Provider304107Standard17Tel",
                                                                   Email =
                                                                       "Provider304107@Standard17ContactEmail.com",
                                                                   Website =
                                                                       "www.Provider304107Standard17ContactURL.com"
                                                               },
                                                       DeliveryLocations =
                                                           new List<DeliveryInformation>
                                                               {
                                                                   new DeliveryInformation
                                                                       {
                                                                           DeliveryLocation
                                                                               =
                                                                               providerLocations
                                                                               .Single(
                                                                                   x =>
                                                                                   x.Id
                                                                                   == 115643),
                                                                           DeliveryModes
                                                                               =
                                                                               new[]
                                                                                   {
                                                                                       ModesOfDelivery
                                                                                           .BlockRelease
                                                                                   },
                                                                           Radius = 80
                                                                       }
                                                               }
                                                   },
                                               new StandardInformation
                                                   {
                                                       Code = 45,
                                                       MarketingInfo = "Provider 304107 marketing into for standard code 45",
                                                       InfoUrl = "www.Provider304107Standard45StandardInfoURL.com",
                                                       ContactInformation =
                                                           new ContactInformation
                                                               {
                                                                   Phone = "Provider304107Standard45Tel",
                                                                   Email =
                                                                       "Provider304107@Standard45ContactEmail.com",
                                                                   Website =
                                                                       "www.Provider304107Standard45ContactURL.com"
                                                               },
                                                       DeliveryLocations =
                                                           new List<DeliveryInformation>
                                                               {
                                                                   new DeliveryInformation
                                                                    {
                                                                        DeliveryLocation
                                                                            =
                                                                            providerLocations
                                                                            .Single(x => x.Id == 115641),
                                                                        DeliveryModes
                                                                            =
                                                                            new[]
                                                                                {
                                                                                    ModesOfDelivery
                                                                                        .OneHundredPercentEmployer
                                                                                },
                                                                        Radius = 80
                                                                    },
                                                                   new DeliveryInformation
                                                                    {
                                                                        DeliveryLocation = providerLocations.Single(x => x.Id == 115640),
                                                                        DeliveryModes
                                                                            =
                                                                            new[]
                                                                                {
                                                                                    ModesOfDelivery
                                                                                        .OneHundredPercentEmployer
                                                                                },
                                                                        Radius = 80
                                                                    }
                                                               }
                                                   }
                                           },
                                   Frameworks = new List<FrameworkInformation>
                                                    {
                                                        new FrameworkInformation
                                                   {
                                                       Code = 45,
                                                       PathwayCode = 7,
                                                       ProgType = 5,
                                                       MarketingInfo = "Provider 304107 marketing into for standard code 45",
                                                       InfoUrl = "www.Provider304107Standard45StandardInfoURL.com",
                                                       ContactInformation =
                                                           new ContactInformation
                                                               {
                                                                   Phone = "Provider304107Standard45Tel",
                                                                   Email =
                                                                       "Provider304107@Standard45ContactEmail.com",
                                                                   Website =
                                                                       "www.Provider304107Standard45ContactURL.com"
                                                               },
                                                       DeliveryLocations =
                                                           new List<DeliveryInformation>
                                                               {
                                                                   new DeliveryInformation
                                                                    {
                                                                        DeliveryLocation = providerLocations.Single(x => x.Id == 115641),
                                                                        DeliveryModes = new[]
                                                                        {
                                                                            ModesOfDelivery.OneHundredPercentEmployer
                                                                        },
                                                                        Radius = 80
                                                                    },
                                                                   new DeliveryInformation
                                                                       {
                                                                           DeliveryLocation = providerLocations.Single(x => x.Id == 115640),
                                                                           DeliveryModes = new[] { ModesOfDelivery.BlockRelease },
                                                                           Radius = 80
                                                                       }
                                                               }
                                                   }
                                                    },
                                   Locations = providerLocations
                               }
                       };
        }
    }
}