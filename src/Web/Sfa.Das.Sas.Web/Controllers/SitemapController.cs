namespace Sfa.Das.Sas.Web.Controllers
{
    using System.Web.Mvc;
    using ApplicationServices.Queries;
    using MediatR;

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
            }).Result;

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
            }).Result;

            return Content(resp.Content, "text/xml");
        }

        public ActionResult Providers()
        {

            var baseUrl = GetBaseUrl();

            var urlPrefix = $"{baseUrl}/provider/{{0}}";

            var resp = _mediator.Send(new SitemapQuery
            {
                UrlPlaceholder = urlPrefix,
                SitemapRequest = SitemapType.Providers
            }).Result;

            return Content(resp.Content, "text/xml");
        }

        private string GetBaseUrl()
        {
               return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty);
        }
    }
}
