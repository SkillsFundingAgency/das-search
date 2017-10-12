using System.Web.Mvc;
using System.Web.Routing;

namespace Sfa.Das.Sas.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Cookies",
                url: "cookies",
                defaults: new { controller = "Start", action = "Cookies", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Stats",
                url: "stats",
                defaults: new { controller = "Stats", action = "Stats", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "GetRobotsText",
                url: "robots.txt",
                defaults: new { controller = "Start", action = "RobotsText", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "GetSitemapXml",
                url: "sitemap.xml",
                defaults: new { controller = "Sitemap", action = "Root", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "ProviderWithName",
                url: "provider/{ukprn}/{providerName}",
                defaults: new { controller = "Provider", action = "ProviderDetail" });

            routes.MapRoute(
                name: "Provider",
                url: "provider/{ukprn}",
                defaults: new { controller = "Provider", action = "ProviderDetail" });

            routes.MapRoute(
                name: "Standard",
                url: "apprenticeship/standard/{id}/{name}",
                defaults: new { controller = "Apprenticeship", action = "Standard", keywords = UrlParameter.Optional });

            routes.MapRoute(
                name: "Framework",
                url: "apprenticeship/framework/{id}/{name}",
                defaults: new { controller = "Apprenticeship", action = "Framework", keywords = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Start", action = "Start", id = UrlParameter.Optional });
        }
    }
}
