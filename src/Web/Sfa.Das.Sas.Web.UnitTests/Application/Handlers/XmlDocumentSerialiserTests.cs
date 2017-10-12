using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Handlers;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public class XmlDocumentSerialiserTests
    {
        private IXmlDocumentSerialiser _sut;
        private const string Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private const string FrameworkPlaceholder = "http://localhost/Sitemap/Frameworks/{0}";

        [SetUp]
        public void Init()
        {
            _sut = new XmlDocumentSerialiser();
        }

        [Test]
        [TestCaseSource(nameof(GetEmptyLists))]
        public void ShouldReturnNoItemsWhenNoneProvided(IEnumerable<string> items)
        {
            var response = _sut.Serialise(Namespace, It.IsAny<string>(), items);

            var doc = XDocument.Parse(response);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(0);
        }

        [Test]
        public void ShouldReturnSerialisedDocument()
        {
            var items = new List<string> { "1" };

            var response = _sut.Serialise(Namespace, FrameworkPlaceholder, items);

            var doc = XDocument.Parse(response);
            XNamespace ns = Namespace;

            var nodes = doc.Descendants(ns + "loc");
            nodes.Count().Should().Be(1);
        }

        private static IEnumerable<List<string>> GetEmptyLists()
        {
            yield return null;
            yield return new List<string>();
        }
    }
}