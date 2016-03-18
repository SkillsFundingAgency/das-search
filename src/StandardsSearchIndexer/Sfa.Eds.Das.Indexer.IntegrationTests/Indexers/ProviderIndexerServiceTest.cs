namespace Sfa.Eds.Das.Indexer.IntegrationTests.Indexers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using Nest;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Eds.Das.Indexer.IntegrationTests.Services;
    using Sfa.Infrastructure.Services;

    using StructureMap;

    [TestFixture]
    public class ProviderIndexerServiceTest
    {
        private IContainer _ioc;

        private IGenericIndexerHelper<Provider> _indexerService;

        private IIndexerService<Provider> _sut;

        private IElasticClient _elasticClient;

        private IIndexSettings<Provider> _providerSettings;

        private Mock<IProviderFeatures> _features;

        [SetUp]
        public void SetUp()
        {
            _ioc = IoC.Initialize();
            _ioc.Configure(x => x.For<IGetApprenticeshipProviders>().Use<StubCourseDirectoryClient>());

            _ioc.GetInstance<IGetApprenticeshipProviders>();
            _ioc.GetInstance<IIndexSettings<Provider>>();
            _indexerService = _ioc.GetInstance<IGenericIndexerHelper<Provider>>();

            _providerSettings = _ioc.GetInstance<IIndexSettings<Provider>>();
            _features = new Mock<IProviderFeatures>();

            var maintainSearchIndexer = _ioc.GetInstance<IMaintainSearchIndexes<Provider>>();
            var providerRepository = new Mock<IGetApprenticeshipProviders>();
            var activeProviderRepository = new Mock<IGetActiveProviders>();

            providerRepository.Setup(m => m.GetApprenticeshipProvidersAsync()).ReturnsAsync(GetProvidersTest());
            _indexerService = new ProviderIndexer(_providerSettings, _features.Object, maintainSearchIndexer, providerRepository.Object, activeProviderRepository.Object, null);

            var elasticClientFactory = _ioc.GetInstance<IElasticsearchClientFactory>();
            _elasticClient = elasticClientFactory.GetElasticClient();

            _sut = _ioc.GetInstance<IIndexerService<Provider>>();
        }

        [Test]
        [Ignore]
        [Category("Integration")]
        public async Task ShouldCreateScheduledIndexAndMappingForProviders()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = $"{_providerSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            await _sut.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<Provider>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        [Test]
        [Category("Integration")]
        [Category("Problematic")]
        [Ignore]
        public void ShouldRetrieveProvidersSearchingForPostCode()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = $"{_providerSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            var providersTest = GetProvidersTest();
            var expectedProviderResult = new Provider
                                             {
                                                 Ukprn = 10002387,
                                                 Name = "F1 COMPUTER SERVICES & TRAINING LIMITED",
                                                 MarketingInfo = "Provider Marketing Information for F1 COMPUTER SERVICES & TRAINING LIMITED",
                                                 ContactDetails = new ContactInformation { Email = "test1@example.com", Website = "http://www.f1training.org.uk", Phone = "01449 770911" }
                                             };

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            _indexerService.IndexEntries(indexName);

            Thread.Sleep(2000);

            var retrievedResult = _elasticClient.Search<Provider>(p => p.Index(indexName).QueryString("MK40 2SG"));
            var amountRetrieved = retrievedResult.Documents.Count();
            var retrievedProvider = retrievedResult.Documents.FirstOrDefault();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            Assert.AreEqual(1, amountRetrieved);
            Assert.AreEqual(expectedProviderResult.Name, retrievedProvider.Name);
        }

        [Test]
        [Category("Integration")]
        [Category("Problematic")]
        [Ignore]
        public void ShouldRetrieveProvidersSearchingForStandardId()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = $"{_providerSettings.IndexesAlias}-{scheduledDate.ToUniversalTime().ToString("yyyy-MM-dd-HH")}".ToLower(CultureInfo.InvariantCulture);

            var providersTest = GetProvidersTest();

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            _indexerService.CreateIndex(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            _indexerService.IndexEntries(indexName);

            Thread.Sleep(2000);

            QueryContainer query1 = new TermQuery { Field = "standardcode", Value = 17 };
            var providersCase1 = _elasticClient.Search<Provider>(s => s.Index(indexName).Query(query1));

            QueryContainer query2 = new TermQuery { Field = "standardcode", Value = 45 };
            var providersCase2 = _elasticClient.Search<Provider>(s => s.Index(indexName).Query(query2));

            QueryContainer query3 = new TermQuery { Field = "standardcode", Value = 1234567890 };
            var providersCase3 = _elasticClient.Search<Provider>(s => s.Index(indexName).Query(query3));

            Assert.AreEqual(1, providersCase1.Documents.Count());
            Assert.AreEqual(1, providersCase2.Documents.Count());
            Assert.AreEqual(0, providersCase3.Documents.Count());

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(i => i.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(i => i.Index(indexName));
            }
        }

        private List<Provider> GetProvidersTest()
        {
            var providerLocations = new List<Location>
                                        {
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
                                   Id = 304107,
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
                                                       StandardCode = 17,
                                                       MarketingInfo = "Provider 304107 marketing into for standard code 17",
                                                       StandardInfoUrl = "www.Provider304107Standard17StandardInfoURL.com",
                                                       StandardContact =
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
                                                       StandardCode = 45,
                                                       MarketingInfo = "Provider 304107 marketing into for standard code 45",
                                                       StandardInfoUrl = "www.Provider304107Standard45StandardInfoURL.com",
                                                       StandardContact =
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
                                                                               .Single(
                                                                                   x =>
                                                                                   x.Id
                                                                                   == 115641),
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
                                   Frameworks = new List<FrameworkInformation>(),
                                   Locations = providerLocations
                               }
                       };
        }
    }
}