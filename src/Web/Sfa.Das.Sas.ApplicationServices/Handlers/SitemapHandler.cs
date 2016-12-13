using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class SitemapHandler : IRequestHandler<SitemapQuery, SitemapResponse>
    {
        private const string SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;

        public SitemapHandler(IGetStandards getStandards, IGetFrameworks getFrameworks)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
        }

        public SitemapResponse Handle(SitemapQuery message)
        {
            var identifiers = new List<string>();

            switch (message.SitemapRequest)
            {
                case SitemapType.Standards:
                    identifiers = _getStandards.GetAllStandards().Select(x => x.StandardId).ToList();
                    break;
                case SitemapType.Frameworks:
                    identifiers = _getFrameworks.GetAllFrameworks().Select(x => $"{x.FrameworkCode}-{x.ProgType}-{x.PathwayCode}").ToList();
                    break;
            }

            var sitemapContents = CreateDocument(identifiers, message.UrlPlaceholder);

            return new SitemapResponse
            {
                Content = sitemapContents.ToString()
            };
        }

        private XDocument CreateDocument(List<string> items, string urlPlaceholder)
        {
            XNamespace ns = SitemapNamespace;

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset",
                items.Select(id => new XElement(ns + "url", new XElement(ns + "loc", string.Format(urlPlaceholder, id))))));
        }
    }
}
