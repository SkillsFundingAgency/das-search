using System.Web.Mvc;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;

namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Reflection;

    using log4net;

    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger("HomeController");
        private readonly ISearchForStandards _searchService;

        public HomeController(ISearchForStandards searchService)
        {
            this._searchService = searchService;
        }

        public ActionResult Index()
        {
            Log.Warn("Hello from!");
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchCriteria criteria)
        {
            var searchResults = _searchService.Search(criteria.Keywords);

            return View(searchResults);
        }
    }
}