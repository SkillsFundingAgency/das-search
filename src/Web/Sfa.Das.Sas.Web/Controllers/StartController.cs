namespace Sfa.Das.Sas.Web.Controllers
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using Core.Configuration;
    using SFA.DAS.NLog.Logger;
    using ViewModels;

    public sealed class StartController : Controller
    {
        private IConfigurationSettings _settings;
        private ILog _log;

        public StartController(
            IConfigurationSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
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

        public ActionResult CookieDetails()
        {
            return View();
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
            else
            {
                builder.AppendLine("Disallow: /apprenticeship/searchforstandardproviders/");
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