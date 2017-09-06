using System.Runtime.CompilerServices;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using MediatR;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using Queries;
    using Responses;
    using Core.Domain.Helpers;
    using Core.Domain.Services;
    public class SitemapHandler : IRequestHandler<SitemapQuery, SitemapResponse>
    {
        private const string SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly IGetProviderDetails _getProviders;
        private readonly IUrlEncoder _urlEncoder;

        public SitemapHandler(IGetStandards getStandards, IGetFrameworks getFrameworks, IGetProviderDetails getProviders, IUrlEncoder urlEncoder)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _getProviders = getProviders;
            _urlEncoder = urlEncoder;
        }

        public SitemapResponse Handle(SitemapQuery message)
        {
            IEnumerable<string> identifiers = null;

            switch (message.SitemapRequest)
            {
                case SitemapType.Standards:
                    identifiers = _getStandards.GetAllStandards().Where(s => s.IsPublished).Select(x => x.StandardId);
                    break;
                case SitemapType.Frameworks:
                    identifiers = _getFrameworks.GetAllFrameworks().Select(x => x.FrameworkId);
                    break;
                case SitemapType.Providers:
                    identifiers = GetProviderDetailsInSeoFormat();
                    break;
                default:
                    break;
            }

            var sitemapContents = CreateDocument(identifiers, message.UrlPlaceholder);

            return new SitemapResponse
            {
                Content = sitemapContents.ToString()
            };
        }

        private IEnumerable<string> GetProviderDetailsInSeoFormat()
        {
            var providersExcludingEmployerProviders = _getProviders.GetAllProviders().Where(x => x.IsEmployerProvider == false);
            var identifiers = BuildProviderSitemapFromProviders(providersExcludingEmployerProviders);
            return identifiers;
        }

        private IEnumerable<string> BuildProviderSitemapFromProviders(IEnumerable<ProviderSummary> providers)
        {
            foreach (var provider in providers)
            {
                var encodedProviderName = _urlEncoder.EncodeTextForUri(provider.ProviderName);
                var urlLocElement = $@"{provider.Ukprn}/{encodedProviderName}";
                yield return urlLocElement;
            }
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
