namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;

    using Sfa.Eds.Das.Core.Interfaces.Search;
    using Sfa.Eds.Das.Core.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public class HomeController : Controller
    {
        private readonly ISearchService searchService;

        private readonly IMappingService mappingService;

        public HomeController(ISearchService searchService, IMappingService mappingService)
        {
            this.searchService = searchService;
            this.mappingService = mappingService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(SearchCriteria criteria)
        {
            var searchResults = this.searchService.SearchByKeyword(criteria.Keywords, criteria.Skip, criteria.Take);

            if (searchResults == null)
            {
                return View(new StandardSearchResultViewModel());
            }

            var viewModel = this.mappingService.Map<SearchResults, StandardSearchResultViewModel>(searchResults);

            return View(viewModel);
        }
    }
}