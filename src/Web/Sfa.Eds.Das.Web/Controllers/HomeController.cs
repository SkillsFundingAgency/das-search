using System.Web.Mvc;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;

namespace Sfa.Eds.Das.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchForStandards _searchService;

        public HomeController(ISearchForStandards searchService)
        {
            this._searchService = searchService;
        }

        public ActionResult Index()
        {
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