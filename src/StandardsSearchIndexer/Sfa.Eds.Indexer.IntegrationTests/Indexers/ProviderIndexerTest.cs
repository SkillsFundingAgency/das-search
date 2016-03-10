using Sfa.Eds.Das.Indexer.Core.Models.ProviderImport;

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
    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Models;

    using StructureMap;

    [TestFixture]
    public class ProviderIndexerTest
    {
        private IContainer _ioc;

        private IGenericIndexerHelper<ProviderOld> _providerHelper;

        private IIndexerService<ProviderOld> _sut;

        private IElasticClient _elasticClient;

        private IIndexMaintenanceService _indexMaintenanceService;

        private IIndexSettings<ProviderOld> _providerSettings;

        [SetUp]
        public void SetUp()
        {
            _ioc = IoC.Initialize();
            _ioc.GetInstance<IIndexSettings<ProviderOld>>();
            _providerHelper = _ioc.GetInstance<IGenericIndexerHelper<ProviderOld>>();
            _providerSettings = _ioc.GetInstance<IIndexSettings<ProviderOld>>();

            var elasticClientFactory = _ioc.GetInstance<IElasticsearchClientFactory>();
            _elasticClient = elasticClientFactory.GetElasticClient();

            _indexMaintenanceService = new IndexMaintenanceService();

            _sut = _ioc.GetInstance<IIndexerService<ProviderOld>>();
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(i => i.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(i => i.Index(indexName));
            }
        }

        private List<ProviderOld> GetProvidersTest()
        {
            return new List<ProviderOld>
            {
                new ProviderOld
                {
                    Id = 304107,
                    UkPrn = "10002387",
                    Name = "F1 COMPUTER SERVICES & TRAINING LIMITED",
                    MarketingInfo = "Provider Marketing Information for F1 COMPUTER SERVICES & TRAINING LIMITED",
                    Email = "test1@example.com",
                    Website = "http://www.f1training.org.uk",
                    Phone = "01449 770911",
                    LearnerSatisfaction = null,
                    EmployerSatisfaction = null,
                    Standards = new List<ProviderStandardInfo>
                    {
                        new ProviderStandardInfo
                        {
                            StandardCode = 17,
                            MarketingInfo = "Provider 304107 marketing into for standard code 17",
                            StandardInfoUrl = "www.Provider304107Standard17StandardInfoURL.com",
                            Contact = new Contact
                            {
                                Phone = "Provider304107Standard17Tel",
                                Email = "Provider304107@Standard17ContactEmail.com",
                                ContactUsUrl = "www.Provider304107Standard17ContactURL.com"
                            },
                            Locations = new List<LocationRef>
                            {
                                new LocationRef
                                {
                                    ID = 115643,
                                    DeliveryModes = new List<string>
                                    {
                                        "BlockRelease"
                                    },
                                    Radius = 80
                                }
                            }
                        },
                        new ProviderStandardInfo
                        {
                            StandardCode = 45,
                            MarketingInfo = "Provider 304107 marketing into for standard code 45",
                            StandardInfoUrl = "www.Provider304107Standard45StandardInfoURL.com",
                            Contact = new Contact
                            {
                                Phone = "Provider304107Standard45Tel",
                                Email = "Provider304107@Standard45ContactEmail.com",
                                ContactUsUrl = "www.Provider304107Standard45ContactURL.com"
                            },
                            Locations = new List<LocationRef>
                            {
                                new LocationRef
                                {
                                    ID = 115641,
                                    DeliveryModes = new List<string>
                                    {
                                        "100PercentEmployer"
                                    },
                                    Radius = 30
                                }
                            }
                        }
                    },
                    Frameworks = new List<ProviderFrameworkInfo>(),
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            ID = 115641,
                            Name = "F1 TRAINING LTD - BEDFORD LEARNING CENTRE",
                            Address = new Address
                            {
                                Address1 = "Enterprise House",
                                Address2 = "2-6 Union Street",
                                Town = "Bedford",
                                County = null,
                                Postcode = "MK40 2SG",
                                Lat = 52.139922,
                                Long = -0.475378
                            }
                        },
                        new Location
                        {
                            ID = 115643,
                            Name = "F1 TRAINING LTD - GT YARMOUTH LEARNING CENTRE",
                            Address = new Address
                            {
                                Address1 = "Catalyst - Business Acceleration Centre",
                                Address2 = "The Conge",
                                Town = "Great Yarmouth",
                                County = null,
                                Postcode = "NR30 1NA",
                                Lat = 52.609776,
                                Long = 1.725685
                            }
                        }
                    }
                }
            };

        }

        [Test]
        [Category("Integration")]
        public async Task ShouldCreateScheduledIndexAndMapping()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledDate, _providerSettings.IndexesAlias);

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            await _sut.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<ProviderOld>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        [Test]
        [Category("Integration")]
        public void ShouldRetrieveProvidersSearchingForPostCode()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledDate, _providerSettings.IndexesAlias);

            var providersTest = GetProvidersTest();
            var expectedProviderResult = new ProviderOld
            {
                UkPrn = "10002387",
                Name = "F1 COMPUTER SERVICES & TRAINING LIMITED",
                MarketingInfo = "Provider Marketing Information for F1 COMPUTER SERVICES & TRAINING LIMITED",
                Email = "test1@example.com",
                Website = "http://www.f1training.org.uk",
                Phone = "01449 770911",
            };

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            _providerHelper.CreateIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            _providerHelper.IndexEntries(scheduledDate, providersTest);

            Thread.Sleep(2000);

            var retrievedResult = _elasticClient.Search<ProviderOld>(p => p.Index(indexName).QueryString("MK40 2SG"));
            var amountRetrieved = retrievedResult.Documents.Count();
            var retrievedProvider = retrievedResult.Documents.FirstOrDefault();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            Assert.AreEqual(1, amountRetrieved);
            Assert.AreEqual(expectedProviderResult.Name, retrievedProvider.Name);
        }

        [Test]
        [Category("Integration")]
        public void ShouldRetrieveProvidersSearchingForStandardId()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = _indexMaintenanceService.GetIndexNameAndDateExtension(scheduledDate, _providerSettings.IndexesAlias);

            var providersTest = GetProvidersTest();

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();

            _providerHelper.CreateIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            _providerHelper.IndexEntries(scheduledDate, providersTest);

            Thread.Sleep(2000);

            QueryContainer query1 = new TermQuery
            {
                Field = "standardcode",
                Value = 17
            };
            var providersCase1 = _elasticClient.Search<ProviderOld>(s => s.Index(indexName).Query(query1));

            QueryContainer query2 = new TermQuery
            {
                Field = "standardcode",
                Value = 45
            };
            var providersCase2 = _elasticClient.Search<ProviderOld>(s => s.Index(indexName).Query(query2));

            QueryContainer query3 = new TermQuery
            {
                Field = "standardcode",
                Value = 1234567890
            };
            var providersCase3 = _elasticClient.Search<ProviderOld>(s => s.Index(indexName).Query(query3));

            Assert.AreEqual(1, providersCase1.Documents.Count());
            Assert.AreEqual(1, providersCase2.Documents.Count());
            Assert.AreEqual(0, providersCase3.Documents.Count());

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }
    }
}