using Microsoft.AspNetCore.Mvc;

namespace Sfa.Das.Sas.Web.Controllers
{
public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}