using System.Web.Mvc;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Web.Controllers
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