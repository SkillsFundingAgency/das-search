using Microsoft.AspNetCore.Mvc;

namespace Sfa.Das.Sas.Shared.Components.Web.Controllers
{
    public class ComponentsController : Controller
    {

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult SearchResults()
        {
            return View();
        }

    }
}
