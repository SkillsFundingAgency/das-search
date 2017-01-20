using System;
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
            IEnumerable<string> identifiers;

            if (message.SitemapRequest == SitemapType.Standards)
            {
                identifiers = _getStandards.GetAllStandards().Where(s => s.IsPublished).Select(x => x.StandardId);
            }
            else
            {
                identifiers = _getFrameworks.GetAllFrameworks().Select(x => x.FrameworkId);
            }

            var sitemapContents = CreateDocument(identifiers, message.UrlPlaceholder);

            return new SitemapResponse
            {
                Content = sitemapContents.ToString()
            };
        }

        private XDocument CreateDocument(IEnumerable<string> items, string urlPlaceholder)
        {
            XNamespace ns = SitemapNamespace;

            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset",
                items.Select(id => new XElement(ns + "url", new XElement(ns + "loc", string.Format(urlPlaceholder, id))))));
        }
    }
}
