namespace Sfa.Infrastructure.UnitTests.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eds.Das.Indexer.Core.Exceptions;
    using Eds.Das.Indexer.Core.Models.Framework;
    using Eds.Das.Indexer.Core.Models.Provider;
    using Infrastructure.Elasticsearch;
    using NUnit.Framework;

    [TestFixture]
    public class ElasticsearchMapperTest
    {
        [Test]
        public void WhenCreatingFrameworkDocument()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 123,
                PwayCode = 1,
                NASTitle = "Sustainable Resource Operations and Management",
                PathwayName = "Higher Apprenticeship in Sustainable Resource Operations and Management",
                ProgType = 20
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Sustainable Resource Operations and Management: Higher Apprenticeship in Sustainable Resource Operations and Management", framework.Title);
            Assert.AreEqual(4, framework.Level, "Should have level");
            Assert.AreEqual("123201", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }

        [Test]
        public void WhenCreatingFrameworkDocumentAndTitleIsPathway()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                NASTitle = "Trade Business Services",
                PathwayName = "Trade Business Services",
                ProgType = 3
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Trade Business Services", framework.Title);
            Assert.AreEqual(2, framework.Level, "Should have level");
            Assert.AreEqual("61631", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }

        [Test]
        public void WhenCreatingFrameworkDocumentAndPathwaySameAsTitleButWithTrailingSpace()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                NASTitle = "Accounting",
                PathwayName = "Accounting ",
                ProgType = 3
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Accounting", framework.Title);
            Assert.AreEqual(2, framework.Level, "Should have level");
            Assert.AreEqual("61631", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }

        [Test]
        public void WhenCreatingFrameworkDocumentAndPathwayIsMissing()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                NASTitle = "Trade Business Services",
                PathwayName = " ",
                ProgType = 23
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Trade Business Services", framework.Title);
            Assert.AreEqual(7, framework.Level, "Should have level");
            Assert.AreEqual("616231", framework.FrameworkId, "Should have id from fwcode, progtype and pwcode");
        }

        [Test]
        [ExpectedException(typeof(MappingException))]
        public void ShouldThrowMappingExceptionOnMappingErrorForFrameworkProviderMapping()
        {
            var mapper = new ElasticsearchMapper(null);
            var testProvider = GenerateTestProvider();

            // Remove Delivery modes
            testProvider.Frameworks.First().DeliveryLocations.First().DeliveryModes = null;

            mapper.CreateFrameworkProviderDocument(testProvider, testProvider.Frameworks.First(), testProvider.Frameworks.First().DeliveryLocations.First());
        }

        [Test]
        [ExpectedException(typeof(MappingException))]
        public void ShouldThrowMappingExceptionOnMappingErrorForStandardProviderMapping()
        {
            var mapper = new ElasticsearchMapper(null);
            var testProvider = GenerateTestProvider();

            // Remove Delivery modes
            testProvider.Standards.First().DeliveryLocations.First().DeliveryModes = null;

            mapper.CreateStandardProviderDocument(testProvider, testProvider.Standards.First(), testProvider.Standards.First().DeliveryLocations.First());
        }

        [Test]
        public void ShouldCreateValidFrameworkProviderDocument()
        {
            var mapper = new ElasticsearchMapper(null);
            var testProvider = GenerateTestProvider();

            var document = mapper.CreateFrameworkProviderDocument(testProvider, testProvider.Frameworks.First(), testProvider.Frameworks.First().DeliveryLocations.First());

            Assert.That(document.FrameworkCode, Is.EqualTo(99));
            Assert.That(document.PathwayCode, Is.EqualTo(1122));
            Assert.That(document.FrameworkId, Is.EqualTo("99201122"));
            Assert.That(document.Level, Is.EqualTo(4));

            Assert.That(document.Ukprn, Is.EqualTo(4556));
            Assert.That(document.Name, Is.EqualTo("Test Provider"));
            Assert.That(document.Id, Is.EqualTo("4556-9941122-77"));
            Assert.That(document.LocationId, Is.EqualTo(77));
            Assert.That(document.LocationName, Is.EqualTo("Framework Test Location"));
            Assert.That(document.ProviderMarketingInfo, Is.EqualTo("Provider Marketing"));
            Assert.That(document.ApprenticeshipMarketingInfo, Is.EqualTo("Framework Apprenticeship Marketing"));
            Assert.That(document.Phone, Is.EqualTo("12324-5678"));
            Assert.That(document.Email, Is.EqualTo("test@test.com"));
            Assert.That(document.ContactUsUrl, Is.EqualTo("http://contact-us.com"));
            Assert.That(document.ApprenticeshipInfoUrl, Is.EqualTo("http://standard-info.com"));
            Assert.That(document.LearnerSatisfaction, Is.EqualTo(8.2));
            Assert.That(document.EmployerSatisfaction, Is.EqualTo(9.2));
            Assert.That(document.DeliveryModes, Is.EquivalentTo(new string[] { "BlockRelease", "DayRelease" }));
            Assert.That(document.Website, Is.EqualTo("http://location-site"));
            Assert.That(document.Address.Address1, Is.EqualTo("Framework Test Address1"));
            Assert.That(document.Address.Address2, Is.EqualTo("Framework Test Address2"));
            Assert.That(document.Address.Town, Is.EqualTo("Framework Test Town"));
            Assert.That(document.Address.County, Is.EqualTo("Framework Test County"));
            Assert.That(document.Address.PostCode, Is.EqualTo("TE3 5ES"));
            Assert.That(document.LocationPoint.Latitude, Is.EqualTo(53.213));
            Assert.That(document.LocationPoint.Longitude, Is.EqualTo(-50.123));
            Assert.That(document.Location.Coordinates.Latitude, Is.EqualTo(53.213));
            Assert.That(document.Location.Coordinates.Longitude, Is.EqualTo(-50.123));
            Assert.That(document.Location.Radius, Is.EqualTo("25mi"));
        }

        [Test]
        public void ShouldCreateValidStandardProviderDocument()
        {
            var mapper = new ElasticsearchMapper(null);
            var testProvider = GenerateTestProvider();

            var document = mapper.CreateStandardProviderDocument(testProvider, testProvider.Standards.First(), testProvider.Standards.First().DeliveryLocations.First());

            Assert.That(document.StandardCode, Is.EqualTo(101));
            Assert.That(document.Ukprn, Is.EqualTo(4556));
            Assert.That(document.Name, Is.EqualTo("Test Provider"));
            Assert.That(document.Id, Is.EqualTo("4556-101-98"));
            Assert.That(document.LocationId, Is.EqualTo(98));
            Assert.That(document.LocationName, Is.EqualTo("Standard Test Location"));
            Assert.That(document.ProviderMarketingInfo, Is.EqualTo("Provider Marketing"));
            Assert.That(document.ApprenticeshipMarketingInfo, Is.EqualTo("Standard Apprenticeship Marketing"));
            Assert.That(document.Phone, Is.EqualTo("5555-5678"));
            Assert.That(document.Email, Is.EqualTo("test@test.com"));
            Assert.That(document.ContactUsUrl, Is.EqualTo("http://contact-us.com"));
            Assert.That(document.ApprenticeshipInfoUrl, Is.EqualTo("http://standard-info.com"));
            Assert.That(document.LearnerSatisfaction, Is.EqualTo(8.2));
            Assert.That(document.EmployerSatisfaction, Is.EqualTo(9.2));
            Assert.That(document.DeliveryModes, Is.EquivalentTo(new string[] { "BlockRelease", "DayRelease" }));
            Assert.That(document.Website, Is.EqualTo("http://location-site"));
            Assert.That(document.Address.Address1, Is.EqualTo("Standard Test Address1"));
            Assert.That(document.Address.Address2, Is.EqualTo("Standard Test Address2"));
            Assert.That(document.Address.Town, Is.EqualTo("Standard Test Town"));
            Assert.That(document.Address.County, Is.EqualTo("Standard Test County"));
            Assert.That(document.Address.PostCode, Is.EqualTo("TE4 5ES"));
            Assert.That(document.LocationPoint.Latitude, Is.EqualTo(54.213));
            Assert.That(document.LocationPoint.Longitude, Is.EqualTo(-52.123));
            Assert.That(document.Location.Coordinates.Latitude, Is.EqualTo(54.213));
            Assert.That(document.Location.Coordinates.Longitude, Is.EqualTo(-52.123));
            Assert.That(document.Location.Radius, Is.EqualTo("30mi"));
        }

        [Test]
        public void WhenCreatingFrameworkDocumentShouldTrimTitleWhiteSpaces()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                NASTitle = " Accounting ",
                PathwayName = "Accounting",
                ProgType = 3
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Accounting", framework.Title);
        }

        [Test]
        public void WhenCreatingFrameworkDocumentShouldTrimPathwayWhiteSpaces()
        {
            var frameworkMetaData = new FrameworkMetaData
            {
                EffectiveFrom = DateTime.Parse("10-Feb-14"),
                EffectiveTo = DateTime.MinValue,
                FworkCode = 616,
                PwayCode = 1,
                NASTitle = "Accounting",
                PathwayName = " Accounting ",
                ProgType = 3
            };

            var mapper = new ElasticsearchMapper(null);

            var framework = mapper.CreateFrameworkDocument(frameworkMetaData);

            Assert.AreEqual("Accounting", framework.PathwayName);
        }

        private Provider GenerateTestProvider()
        {
            var provider = new Provider
            {
                Id = "1234",
                Ukprn = 4556,
                Name = "Test Provider",
                MarketingInfo = "Provider Marketing",
                LearnerSatisfaction = 8.2,
                EmployerSatisfaction = 9.2,
                Frameworks = new List<FrameworkInformation>
                {
                    new FrameworkInformation
                    {
                        Code = 99,
                        PathwayCode = 1122,
                        ProgType = 20,
                        MarketingInfo = "Framework Apprenticeship Marketing",
                        InfoUrl = "http://standard-info.com",
                        ContactInformation = new ContactInformation
                        {
                            Phone = "12324-5678",
                            Email = "test@test.com",
                            Website = "http://contact-us.com"
                        },
                        DeliveryLocations = new List<DeliveryInformation>
                        {
                            new DeliveryInformation
                            {
                                DeliveryLocation = new Location
                                {
                                    Id = 77,
                                    Name = "Framework Test Location",
                                    Address = new Eds.Das.Indexer.Core.Models.Provider.Address
                                    {
                                        Address1 = "Framework Test Address1",
                                        Address2 = "Framework Test Address2",
                                        Town = "Framework Test Town",
                                        County = "Framework Test County",
                                        Postcode = "TE3 5ES",
                                        GeoPoint = new Eds.Das.Indexer.Core.Models.Coordinate { Latitude = 53.213, Longitude = -50.123 }
                                    },
                                    Contact = new ContactInformation
                                    {
                                        Website = "http://location-site"
                                    }
                                },
                                Radius = 25,
                                DeliveryModes = new List<ModesOfDelivery> { ModesOfDelivery.BlockRelease, ModesOfDelivery.DayRelease }
                            }
                        }
                    }
                },
                Standards = new List<StandardInformation>
                {
                    new StandardInformation
                    {
                        Code = 101,
                        MarketingInfo = "Standard Apprenticeship Marketing",
                        InfoUrl = "http://standard-info.com",
                        ContactInformation = new ContactInformation
                        {
                            Phone = "5555-5678",
                            Email = "test@test.com",
                            Website = "http://contact-us.com"
                        },
                        DeliveryLocations = new List<DeliveryInformation>
                        {
                            new DeliveryInformation
                            {
                                DeliveryLocation = new Location
                                {
                                    Id = 98,
                                    Name = "Standard Test Location",
                                    Address = new Eds.Das.Indexer.Core.Models.Provider.Address
                                    {
                                        Address1 = "Standard Test Address1",
                                        Address2 = "Standard Test Address2",
                                        Town = "Standard Test Town",
                                        County = "Standard Test County",
                                        Postcode = "TE4 5ES",
                                        GeoPoint = new Eds.Das.Indexer.Core.Models.Coordinate { Latitude = 54.213, Longitude = -52.123 }
                                    },
                                    Contact = new ContactInformation
                                    {
                                        Website = "http://location-site"
                                    }
                                },
                                Radius = 30,
                                DeliveryModes = new List<ModesOfDelivery> { ModesOfDelivery.BlockRelease, ModesOfDelivery.DayRelease }
                            }
                        }
                    }
                }
            };

            return provider;
        }
    }
}
