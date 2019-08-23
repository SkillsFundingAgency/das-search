using System;
using System.Threading;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Core.Domain.Helpers;
    using Core.Domain.Model;
    using Core.Domain.Services;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices.Handlers;
    using Sas.ApplicationServices.Queries;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

    [TestFixture]
    public sealed class SitemapHandlerTest
    {
        private const string StandardPlaceholder = "http://localhost/Sitemap/Standards/{0}";

        private const string FrameworkPlaceholder = "http://localhost/Sitemap/Frameworks/{0}";

        private const string ProviderPlaceholder = "http://localhost/providers/{0}";

        private const string Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

        private SitemapHandler _sut;

        private Mock<IGetStandards> _mockGetStandards;

        private Mock<IGetFrameworks> _mockGetFrameworks;

        private Mock<IGetProviderDetails> _mockProviderDetailRepository;

        private Mock<IUrlEncoder> _mockUrlEncoder;

        private Mock<IXmlDocumentSerialiser> _mockDocumentCreator;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();
            _mockProviderDetailRepository = new Mock<IGetProviderDetails>();
            _mockUrlEncoder = new Mock<IUrlEncoder>();
            _mockDocumentCreator = new Mock<IXmlDocumentSerialiser>();

            _sut = new SitemapHandler(_mockGetStandards.Object, _mockGetFrameworks.Object, _mockProviderDetailRepository.Object, _mockUrlEncoder.Object, _mockDocumentCreator.Object);
        }

        [Test]
        public void ShouldReturnXmlSitemapWithCorrectLinksToIndividualStandards()
        {
            _mockGetStandards.Setup(x => x.GetAllStandards()).Returns(new List<Standard>
            {
                new Standard { StandardId = "23", IsPublished = true, EffectiveFrom = DateTime.Today},
                new Standard { StandardId = "43", IsPublished = true, EffectiveFrom = DateTime.Today}
            });

            var items = new List<string> { "23", "43" };

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), StandardPlaceholder, items))
                .Returns(CreateDocument(Namespace, StandardPlaceholder, items));

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = StandardPlaceholder, SitemapRequest = SitemapType.Standards }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(2);
            nodes.ElementAt(0).Value.Should().Be("http://localhost/Sitemap/Standards/23");
            nodes.ElementAt(1).Value.Should().Be("http://localhost/Sitemap/Standards/43");
        }

        [Test]
        public void ShouldReturnXmlSitemapWithCorrectLinksToIndividualFrameworks()
        {
            _mockGetFrameworks.Setup(x => x.GetAllFrameworks()).Returns(new List<Framework>
            {
                new Framework
                {
                    FrameworkId = "23-5-7", EffectiveFrom = DateTime.Today
                }
            });

            var items = new List<string> { "23-5-7" };

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), FrameworkPlaceholder, items))
                .Returns(CreateDocument(Namespace, FrameworkPlaceholder, items));

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = FrameworkPlaceholder, SitemapRequest = SitemapType.Frameworks }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(1);
            nodes.ElementAt(0).Value.Should().Be("http://localhost/Sitemap/Frameworks/23-5-7");
        }

        [Test]
        public void ShouldReturnXmlSitemapWithCorrectLinksToIndividualNonEmployerProviders()
        {
            var providerNameToProcess = "test name 2";
            var providerNameProcessed = "test-name-2";

            var ukprnToProcess = 1111111;

            _mockProviderDetailRepository.Setup(x => x.GetAllProviders()).Returns(new List<ProviderSummary>
            {
                new ProviderSummary
                {
                    Ukprn = 11111,
                    ProviderName = "test name",
                    IsEmployerProvider = true
                },
                new ProviderSummary
                {
                    Ukprn = ukprnToProcess,
                    ProviderName = providerNameToProcess,
                    IsEmployerProvider = false
                }
            });

            var items = new List<string> { $"{ukprnToProcess}/{providerNameProcessed}" };

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), ProviderPlaceholder, items))
                .Returns(CreateDocument(Namespace, ProviderPlaceholder, items));

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(providerNameToProcess)).Returns(providerNameProcessed);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = ProviderPlaceholder, SitemapRequest = SitemapType.Providers },default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(1);
            nodes.ElementAt(0).Value.Should().Be($"http://localhost/providers/{ukprnToProcess}/{providerNameProcessed}");
        }

        [Test]
        public void ShouldReturnXmlSitemapWithStandardNames()
        {
            var standardOneTitle = "Standard One";
            var standardOneEncoded = "standard-one";

            _mockGetStandards.Setup(x => x.GetAllStandards()).Returns(new List<Standard>
            {
                new Standard { StandardId = "1", Title = standardOneTitle, IsPublished = true, EffectiveFrom = DateTime.Today },
                new Standard { StandardId = "2", IsPublished = true, EffectiveFrom = DateTime.Today }
            });

            const string urlPrefix = "http://localhost/Sitemap/";

            var items = new List<string> { $"1/{standardOneEncoded}", "2" };

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), StandardPlaceholder, items))
                .Returns(CreateDocument(Namespace, StandardPlaceholder, items));

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(standardOneTitle)).Returns(standardOneEncoded);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = StandardPlaceholder, SitemapRequest = SitemapType.Standards }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(2);
            nodes.ElementAt(0).Value.Should().Be($"{urlPrefix}Standards/1/{standardOneEncoded}");
            nodes.ElementAt(1).Value.Should().Be($"{urlPrefix}Standards/2");
        }

        [Test]
        public void ShouldReturnNoResultsWhenStandardNotPublished()
        {
            var standardOneTitle = "Standard One";
            var standardOneEncoded = "standard-one";

            _mockGetStandards.Setup(x => x.GetAllStandards()).Returns(new List<Standard>
            {
                new Standard { StandardId = "1", Title = standardOneTitle, IsPublished = false},
            });

            const string urlPrefix = "http://localhost/Sitemap/";

            var items = new List<string>();

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), StandardPlaceholder, items))
                .Returns(CreateDocument(Namespace, StandardPlaceholder, items));

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(standardOneTitle)).Returns(standardOneEncoded);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = StandardPlaceholder, SitemapRequest = SitemapType.Standards }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(0);
        }

        [Test]
        public void ShouldReturnXmlSitemapWithFrameworkNames()
        {
            var frameworkOneTitle = "Framework One";
            var frameworkOneEncoded = "framework-one";

            _mockGetFrameworks.Setup(x => x.GetAllFrameworks()).Returns(new List<Framework>
            {
                new Framework { FrameworkId = "1", Title = frameworkOneTitle, EffectiveFrom = DateTime.Today},
                new Framework { FrameworkId = "2", EffectiveFrom = DateTime.Today}
            });

            const string urlPrefix = "http://localhost/Sitemap/";

            var items = new List<string> { $"1/{frameworkOneEncoded}", "2"};

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), FrameworkPlaceholder, items))
                .Returns(CreateDocument(Namespace, FrameworkPlaceholder, items));

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(frameworkOneTitle)).Returns(frameworkOneEncoded);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = FrameworkPlaceholder, SitemapRequest = SitemapType.Frameworks }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(2);
            nodes.ElementAt(0).Value.Should().Be($"{urlPrefix}Frameworks/1/{frameworkOneEncoded}");
            nodes.ElementAt(1).Value.Should().Be($"{urlPrefix}Frameworks/2");
        }

        [TestCaseSource("EffectiveDatesTestData")]
        public void ShouldReturnXmlSitemapStandardsConsideringEffectiveFromAndToDates(DateTime? effectiveFrom, DateTime? effectiveTo, int incrementIncrease)
        {
            _mockGetStandards.Setup(x => x.GetAllStandards()).Returns(new List<Standard>
            {
                new Standard { StandardId = "23", IsPublished = true, EffectiveFrom = DateTime.Today.AddDays(-1) },
                new Standard { StandardId = "43", IsPublished = true, EffectiveFrom = DateTime.Today.AddDays(-1) },
                new Standard { StandardId = "63", IsPublished = true, EffectiveFrom = effectiveFrom, EffectiveTo = effectiveTo }
            });

            var items = incrementIncrease == 1 ?
                new List<string> { "23", "43", "63" } :
                new List<string> { "23", "43" };

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), StandardPlaceholder, items))
                .Returns(CreateDocument(Namespace, StandardPlaceholder, items));

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = StandardPlaceholder, SitemapRequest = SitemapType.Standards }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(2 + incrementIncrease);
            nodes.ElementAt(0).Value.Should().Be("http://localhost/Sitemap/Standards/23");
            nodes.ElementAt(1).Value.Should().Be("http://localhost/Sitemap/Standards/43");
        }

        [TestCaseSource("EffectiveDatesTestData")]
        public void ShouldReturnXmlSitemapFrameworksConsideringEffectiveFromAndToDates(DateTime? effectiveFrom, DateTime? effectiveTo, int incrementIncrease)
        {
            _mockGetFrameworks.Setup(x => x.GetAllFrameworks()).Returns(new List<Framework>
            {
                new Framework
                {
                    FrameworkId = "23-5-7", EffectiveFrom = DateTime.Today
                },
                new Framework { FrameworkId = "23-6-7", EffectiveFrom = effectiveFrom, EffectiveTo = effectiveTo }
            });

            var items = incrementIncrease == 1 ?
                new List<string> { "23-5-7", "23-6-7" } :
                new List<string> { "23-5-7" };

            _mockDocumentCreator.Setup(x => x.Serialise(It.IsAny<string>(), FrameworkPlaceholder, items))
                .Returns(CreateDocument(Namespace, FrameworkPlaceholder, items));

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = FrameworkPlaceholder, SitemapRequest = SitemapType.Frameworks }, default(CancellationToken)).Result;

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(1 + incrementIncrease);
            nodes.ElementAt(0).Value.Should().Be("http://localhost/Sitemap/Frameworks/23-5-7");
        }

        private static readonly object[] EffectiveDatesTestData =
        {
            new object[] {DateTime.Today.AddDays(-1), null, 1 },
            new object[] {DateTime.Today.AddDays(1), null, 0 },
            new object[] { null, null, 0 },
            new object[] { DateTime.Today.AddDays(-10), DateTime.Today, 0 },
            new object[] {DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), 1 },

        };

        private string CreateDocument(string xmlNamespace, string urlPlaceholder, IEnumerable<string> items)
        {
            XNamespace ns = xmlNamespace;

            var document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(ns + "urlset", items.Select(id => new XElement(ns + "url", new XElement(ns + "loc", string.Format(urlPlaceholder, id))))));

            return document.ToString();
        }
    }
}
