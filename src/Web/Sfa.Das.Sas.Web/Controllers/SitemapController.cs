using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Web.Services;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class SitemapController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IProviderService _providerService;

        public SitemapController(IMediator mediator, IProviderService providerService)
        {
            _mediator = mediator;
            _providerService = providerService;
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
            0
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

        public ActionResult Providers()
        {
            var providers = _providerService.GetProviderList();

            var builder = BuildProviderSitemapFromDictionary(providers);

            return Content(builder, "text/xml");
        }

        public string ModifyProviderNameForUrl(string providerName)
        {
            var firstpass = providerName.ToLower().Replace("&", "and").Replace("+", "and").Replace(" ", "-");
            var secondpass = new StringBuilder();
            foreach (var c in firstpass)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || c == '-')
                {
                    secondpass.Append(c);
                }
            }

            var thirdpass = secondpass.ToString();

            while (thirdpass.Contains("--"))
            {
                thirdpass = thirdpass.Replace("--", "-");
            }

            return thirdpass;
        }

        private string BuildProviderSitemapFromDictionary(Dictionary<long, string> providers)
        {
            var builder = new StringBuilder();
            var baseUrl = GetBaseUrl();

            builder.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

            foreach (var provider in providers)
            {
                var modifiedProviderName = ModifyProviderNameForUrl(provider.Value);
                var urlLocElement = BuildUrlLocElementFromDetails(baseUrl, "provider", provider.Key, modifiedProviderName);
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
            return Request == null || Request.Url == null ?
                string.Empty :
                Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty);
        }
    }
}
