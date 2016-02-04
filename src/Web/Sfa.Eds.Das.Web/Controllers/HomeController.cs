namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Sfa.Eds.Das.Core.Interfaces.Search;
    using Sfa.Eds.Das.Core.Models;
    using Sfa.Eds.Das.Web.ViewModels;

    public class HomeController : Controller
    {
        private readonly ISearchService searchService;
        public HomeController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(SearchCriteria criteria)
        {
            var searchResults = this.searchService.SearchByKeyword(criteria.Keywords);
            var viewModel = new StandardSearchResultViewModel // AutoMapper
                                {
                                    TotalResults = searchResults.TotalResults,
                                    SearchTerm = searchResults.SearchTerm,
                                    Results =
                                        searchResults.Results.Select(
                                            m =>
                                            new StandardResultItemViewModel
                                                {
                                                    StandardId = m.StandardId,
                                                    NotionalEndLevel = m.NotionalEndLevel,
                                                    Title = m.Title
                                                })
                                };
            return View(viewModel);
        }
    }
}