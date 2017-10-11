using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Core.Domain.Helpers;
    using Core.Domain.Services;
    using MediatR;
    using Queries;
    using Responses;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

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
                    identifiers = GetStandardDetailsInSeoFormat();
                    break;
                case SitemapType.Frameworks:
                    identifiers = GetFrameworkDetailsInSeoFormat();
                    break;
                case SitemapType.Providers:
                    identifiers = GetProviderDetailsInSeoFormat();
                    break;
            }

            var sitemapContents = CreateDocument(identifiers, message.UrlPlaceholder);

            return new SitemapResponse
            {
                Content = sitemapContents.ToString()
            };
        }

        private IEnumerable<string> GetFrameworkDetailsInSeoFormat()
        {
            var frameworks = _getFrameworks.GetAllFrameworks();

            return BuildFrameworkSitemap(frameworks);
        }

        private IEnumerable<string> GetStandardDetailsInSeoFormat()
        {
            var standards = _getStandards.GetAllStandards().Where(s => s.IsPublished);

            return BuildStandardSitemap(standards);
        }

        private IEnumerable<string> GetProviderDetailsInSeoFormat()
        {
            var providersExcludingEmployerProviders = _getProviders.GetAllProviders().Where(x => x.IsEmployerProvider == false);

            return BuildProviderSitemapFromProviders(providersExcludingEmployerProviders);
        }

        private IEnumerable<string> BuildFrameworkSitemap(IEnumerable<Framework> frameworks)
        {
            foreach (var framework in frameworks)
            {
                var title = EncodeTitle(framework);
                yield return GetSeoFormat(framework.FrameworkId, title);
            }
        }

        private IEnumerable<string> BuildStandardSitemap(IEnumerable<Standard> standards)
        {
            foreach (var standard in standards)
            {
                var title = EncodeTitle(standard);
                yield return GetSeoFormat(standard.StandardId, title);
            }
        }

        private IEnumerable<string> BuildProviderSitemapFromProviders(IEnumerable<ProviderSummary> providers)
        {
            foreach (var provider in providers)
            {
                var encodedProviderName = _urlEncoder.EncodeTextForUri(provider.ProviderName);
                yield return $@"{provider.Ukprn}/{encodedProviderName}";
            }
        }

        private string GetSeoFormat(string id, string title)
        {
            return string.IsNullOrEmpty(title) ? $"{id}" : $"{id}/{title}";
        }

        private string EncodeTitle(IApprenticeshipProduct product)
        {
            return _urlEncoder.EncodeTextForUri(product.Title);
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
