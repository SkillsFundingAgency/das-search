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
    }
}