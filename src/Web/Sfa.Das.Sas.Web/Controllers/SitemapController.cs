using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class SitemapController : Controller
    {
        private readonly IMediator _mediator;

        public SitemapController(IMediator mediator)
        {
            _mediator = mediator;
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

        public ActionResult Providers()
        {
            var baseUrl = GetBaseUrl();
            var providers = new SFA.DAS.Providers.Api.Client.ProviderApiClient();
            var res = providers.FindAll()
                .ToDictionary(x => x.Ukprn, x => x.ProviderName);

            var builder = new StringBuilder();
            builder.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

            foreach (var provider in res)
            {
                var modifiedProviderName = ModifyProviderNameForUrl(provider.Value);
                var details = $"{baseUrl}/provider/{provider.Key}/{modifiedProviderName}";
                var urlLocElement = BuildUrlLocElement(details);
                builder.AppendLine(urlLocElement);
            }

            builder.AppendLine(@"</urlset>");

            return Content(builder.ToString(), "text/xml");
        }

        private static string BuildUrlLocElement(string details)
        {
            var item = new StringBuilder();
            item.AppendLine(@"<url>");
            item.AppendLine(@"<loc>");
            item.AppendLine(details);
            item.AppendLine(@"</loc>");
            item.AppendLine(@"</url>");
            return item.ToString();
        }

        private static string ModifyProviderNameForUrl(string providerName)
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

        private string GetBaseUrl()
        {
            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty);
        }
    }
}
