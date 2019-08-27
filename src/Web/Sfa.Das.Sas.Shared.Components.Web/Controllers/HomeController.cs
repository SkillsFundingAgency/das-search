using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Web.Models;

namespace Sfa.Das.Sas.Shared.Components.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchComponent()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
