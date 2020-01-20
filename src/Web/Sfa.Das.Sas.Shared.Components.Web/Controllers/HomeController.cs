using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Web.Models;
using Sfa.Das.Sas.Shared.Components.Web.ViewModel;

namespace Sfa.Das.Sas.Shared.Components.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILayoutService _layoutService;

        public HomeController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

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

        public IActionResult Layout(LayoutViewModel model)
        {
            _layoutService.Layout = model.Layout;
            return LocalRedirect(model.ReturnUrl);
        }
    }
}
