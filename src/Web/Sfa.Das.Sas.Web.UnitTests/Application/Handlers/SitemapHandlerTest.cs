using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public sealed class SitemapHandlerTest
    {
        private SitemapHandler _sut;
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IGetFrameworks> _mockGetFrameworks;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();

            _sut = new SitemapHandler(_mockGetStandards.Object, _mockGetFrameworks.Object);
        }

        [Test]
        public void ShouldReturnXmlSitemapWithCorrectLinksToIndividualStandards()
        {
            _mockGetStandards.Setup(x => x.GetAllStandards()).Returns(new List<Standard>
            {
                new Standard {StandardId = "23"},
                new Standard {StandardId = "43"}
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
    }
}
