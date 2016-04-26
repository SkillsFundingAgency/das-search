using System.Web.Mvc;
using Sfa.Eds.Das.Core.Configuration;

namespace Sfa.Eds.Das.Web.Controllers
{
    public class SharedController : Controller
    {
        private readonly string _surveyUrl;

        public SharedController(IConfigurationSettings settings)
        {
            _surveyUrl = settings.SurveyUrl.ToString();
        }

        public PartialViewResult Footer()
        {
            return PartialView("_footer", _surveyUrl);
        }

        public PartialViewResult Header()
        {
            return PartialView("_Header", _surveyUrl);
        }
    }
}