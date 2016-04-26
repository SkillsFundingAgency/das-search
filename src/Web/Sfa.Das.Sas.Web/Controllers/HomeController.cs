using System.Web.Mvc;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}