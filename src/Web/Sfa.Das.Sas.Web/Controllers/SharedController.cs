using System.Web.Mvc;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Web.Controllers
{
    using Sfa.Das.Sas.Web.Services;
    using Sfa.Das.Sas.Web.ViewModels;

    public class SharedController : Controller
    {
        private readonly ICookieService _cookieService;
        private readonly string _surveyUrl;

        public SharedController(IConfigurationSettings settings, ICookieService cookieService)
        {
            _surveyUrl = settings.SurveyUrl.ToString();
            _cookieService = cookieService;
        }

        public PartialViewResult Footer()
        {
            return PartialView("_footer", _surveyUrl);
        }

        public PartialViewResult Header()
        {
            var viewModel = new HeaderViewModel
                                {
                                    SurveyUrl = _surveyUrl,
                                    ShowCookieBanner = _cookieService.ShowCookieForBanner(Request?.RequestContext?.HttpContext)
                                };

            return PartialView("_Header", viewModel);
        }
    }
}