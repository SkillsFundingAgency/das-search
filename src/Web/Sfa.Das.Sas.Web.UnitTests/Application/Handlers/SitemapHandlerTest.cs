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
        private SitemapHandler _sut;
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IGetFrameworks> _mockGetFrameworks;
        private Mock<IGetProviderDetails> _mockProviderDetailRepository;
        private Mock<IUrlEncoder> _mockUrlEncoder;
        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();
            _mockProviderDetailRepository = new Mock<IGetProviderDetails>();
            _mockUrlEncoder = new Mock<IUrlEncoder>();

            _sut = new SitemapHandler(_mockGetStandards.Object, _mockGetFrameworks.Object, _mockProviderDetailRepository.Object, _mockUrlEncoder.Object);
        }

        [Test]
        public void ShouldReturnXmlSitemapWithCorrectLinksToIndividualStandards()
        {
            _mockGetStandards.Setup(x => x.GetAllStandards()).Returns(new List<Standard>
            {
                new Standard {StandardId = "23", IsPublished = true},
                new Standard {StandardId = "43", IsPublished = true}
            });

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = "http://localhost/Sitemap/Standards/{0}", SitemapRequest = SitemapType.Standards});

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

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
                    FrameworkId = "23-5-7"
                }
            });

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = "http://localhost/Sitemap/Frameworks/{0}", SitemapRequest = SitemapType.Frameworks });

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

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

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(providerNameToProcess)).Returns(providerNameProcessed);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = "http://localhost/providers/{0}", SitemapRequest = SitemapType.Providers });

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

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
                new Standard {StandardId = "1", Title = standardOneTitle, IsPublished = true},
                new Standard {StandardId = "2", IsPublished = true}
            });

            const string urlPrefix = "http://localhost/Sitemap/";

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(standardOneTitle)).Returns(standardOneEncoded);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = $"{urlPrefix}Standards/{{0}}", SitemapRequest = SitemapType.Standards });

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(2);
            nodes.ElementAt(0).Value.Should().Be($"{urlPrefix}Standards/1/{standardOneEncoded}");
            nodes.ElementAt(1).Value.Should().Be($"{urlPrefix}Standards/2");
        }

        [Test]
        public void ShouldReturnXmlSitemapWithFrameworkNames()
        {
            var frameworkOneTitle = "Framework One";
            var frameworkOneEncoded = "framework-one";

            _mockGetFrameworks.Setup(x => x.GetAllFrameworks()).Returns(new List<Framework>
            {
                new Framework {FrameworkId = "1", Title = frameworkOneTitle},
                new Framework {FrameworkId = "2"}
            });

            const string urlPrefix = "http://localhost/Sitemap/";

            _mockUrlEncoder.Setup(x => x.EncodeTextForUri(frameworkOneTitle)).Returns(frameworkOneEncoded);

            var response = _sut.Handle(new SitemapQuery { UrlPlaceholder = $"{urlPrefix}Frameworks/{{0}}", SitemapRequest = SitemapType.Frameworks });

            var doc = XDocument.Parse(response.Content);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(2);
            nodes.ElementAt(0).Value.Should().Be($"{urlPrefix}Frameworks/1/{frameworkOneEncoded}");
            nodes.ElementAt(1).Value.Should().Be($"{urlPrefix}Frameworks/2");
        }

    }
}
