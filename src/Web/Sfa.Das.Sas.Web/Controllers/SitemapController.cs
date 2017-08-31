using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Domain.Repositories;
using Sfa.Das.Sas.Infrastructure.Repositories;
using Sfa.Das.Sas.Web.Helpers;
using Sfa.Das.Sas.Web.Services;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class SitemapController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IProviderDetailRepository _providerDetailRepository;
        private readonly IUrlEncoder _urlEncoder;

        public SitemapController(IMediator mediator, IProviderDetailRepository providerDetailRepository, IUrlEncoder encoder)
        {
            _mediator = mediator;
            _providerDetailRepository = providerDetailRepository;
            _urlEncoder = encoder;
        }

        public ActionResult Root()
        {
            var baseUrl = GetBaseUrl();

            var content = $@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<sitemapindex xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
     <sitemap>
       <loc>{baseUrl}/sitemap/standards</loc>
     </sitemap>
     <sitemap>
       <loc>{baseUrl}/sitemap/frameworks</loc>
     </sitemap>
     <sitemap>
       <loc>{baseUrl}/sitemap/providers</loc>
     </sitemap>
</sitemapindex>";

            return Content(content, "text/xml");
        }

        // GET: Sitemap/Standards
        public ActionResult Standards()
        {
            var baseUrl = GetBaseUrl();

            var urlPrefix = $"{baseUrl}{Url.Action("Standard", "Apprenticeship", new { id = "" })}/{{0}}";

            var resp = _mediator.Send(new SitemapQuery
            {
                UrlPlaceholder = urlPrefix,
                SitemapRequest = SitemapType.Standards
            });

            return Content(resp.Content, "text/xml");
        }

        // GET: Sitemap/Frameworks
        public ActionResult Frameworks()
        {
            var baseUrl = GetBaseUrl();

            var urlPrefix = $"{baseUrl}{Url.Action("Framework", "Apprenticeship", new { id = "" })}/{{0}}";

            var resp = _mediator.Send(new SitemapQuery
            {
                UrlPlaceholder = urlPrefix,
                SitemapRequest = SitemapType.Frameworks
            });

            return Content(resp.Content, "text/xml");
        }

        public async Task<ActionResult> Providers()
        {
            var providers = await _providerDetailRepository.GetProviderList();

            var builder = BuildProviderSitemapFromDictionary(providers);

            return Content(builder, "text/xml");
        }

        private string BuildProviderSitemapFromDictionary(IEnumerable<ProviderSummary> providers)
        {
            var builder = new StringBuilder();
            var baseUrl = GetBaseUrl();

            builder.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

            foreach (var provider in providers)
            {
                var modifiedProviderName = _urlEncoder.EncodeTextForUri(provider.ProviderName);
                var urlLocElement = BuildUrlLocElementFromDetails(baseUrl, "provider", provider.Ukprn, modifiedProviderName);
                builder.AppendLine(urlLocElement);
            }

            builder.Append(@"</urlset>");
            return builder.ToString();
        }

        private string BuildUrlLocElementFromDetails(string baseUrl,string grouping, long key, string modifiedProviderName)
        {
              var details = $"{baseUrl}/{grouping}/{key}/{modifiedProviderName}";

            return $@"  <url>
    <loc>
      {details}
    </loc>
  </url>";
        }

        private string GetBaseUrl()
        {
               return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty);
        }
    }
}
