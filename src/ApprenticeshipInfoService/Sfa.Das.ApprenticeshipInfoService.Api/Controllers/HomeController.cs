using System.Web.Mvc;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/swagger/ui/index");
        }
    }
}