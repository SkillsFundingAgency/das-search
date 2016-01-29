
namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;
    using Models;
    using Services;

    public class HomeController : Controller
    {
        private readonly ISearchForStandards searchService;

        public HomeController(ISearchForStandards searchService)
        {
            this.searchService = searchService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchCriteria criteria)
        {
            var searchResults = this.searchService.Search(criteria.Keywords);

            return View(searchResults);
        }
    }
}