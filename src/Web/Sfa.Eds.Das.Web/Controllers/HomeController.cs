namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;

    using log4net;

    using Models;
    using Services;

    public class HomeController : Controller
    {
        private readonly ILog logger;

        private readonly ISearchForStandards searchService;
        
        public HomeController(ISearchForStandards searchService, ILog logger)
        {
            this.searchService = searchService;
            this.logger = logger;
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

        public ActionResult StandardDetail(string id)
        {
            var standardResult = this.searchService.GetStandardItem(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                this.logger.Warn($"404 - {message}");
                return new HttpNotFoundResult(message);
            }

            return View(standardResult);
        }
    }
}