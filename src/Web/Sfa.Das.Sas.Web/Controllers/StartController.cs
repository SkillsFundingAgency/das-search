using System;
using System.Text;
using System.Web.Mvc;
using MediatR;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class StartController : Controller
    {
        private IConfigurationSettings _settings;

        public StartController(
            IConfigurationSettings settings)
        {
            _settings = settings;
        }

        public ActionResult Start()
        {
            return View();
        }

        public ActionResult Cookies()
        {
            var cookieViewModel = new CookieViewModel
            {
                ImprovementUrl = _settings.CookieImprovementUrl.ToString(),
                GoogleUrl = _settings.CookieGoogleUrl.ToString(),
                ApplicationInsightsUrl = _settings.CookieApplicationInsightsUrl.ToString(),
                AboutUrl = _settings.CookieAboutUrl.ToString(),
                SurveyProviderUrl = _settings.SurveyProviderUrl.ToString()
            };
            
            return View(cookieViewModel);
        }
        
        [OutputCache(Duration = 86400)]
        public ContentResult RobotsText()
        {
            var baseUrl = GetBaseUrl();
            var builder = new StringBuilder();

            builder.AppendLine("User-agent: *");

            if (!_settings.EnvironmentName.Equals("Prod", StringComparison.OrdinalIgnoreCase))
            {
                builder.AppendLine("Disallow: /");
            }

            builder.AppendLine($"Sitemap: {baseUrl}/sitemap.xml");

            return Content(builder.ToString(), "text/plain", Encoding.UTF8);
        }

        private string GetBaseUrl()
        {
            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty);
        }
    }
}