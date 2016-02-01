namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;
    using Models;
    using Services;

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
            var searchResults = this._searchService.Search(criteria.Keywords);

            return View(searchResults);
        }

        public ActionResult StandardDetail(string id)
        {
            var standardResult = this._searchService.GetStandardItem(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                return new HttpNotFoundResult(message);
            }

            return View(standardResult);
        }
    }
}