namespace Sfa.Eds.Das.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using log4net;

    using Sfa.Eds.Das.Core.Interfaces.Search;
    using Sfa.Eds.Das.Core.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public class StandardController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;

        public StandardController(ISearchService searchService, ILog logger, IMappingService mappingService)
        {
            _searchService = searchService;
            _logger = logger;
            _mappingService = mappingService;
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchResults(SearchCriteria criteria)
        {
            var searchResults = _searchService.SearchByKeyword(criteria.Keywords, criteria.Skip, criteria.Take);

            var viewModel = _mappingService.Map<StandardSearchResults, StandardSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        // GET: Standard
        public ActionResult Detail(string id)
        {
            var standardResult = _searchService.GetStandardItem(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                _logger.Warn($"404 - {message}");
                return new HttpNotFoundResult(message);
            }

            var viewModel = _mappingService.Map<StandardSearchResultsItem, StandardViewModel>(standardResult);
            viewModel.SearchResultLink = GetSearchResultUrl(Request.UrlReferrer);
            return View(viewModel);
        }

        private LinkViewModel GetSearchResultUrl(Uri urlReferrer)
        {
            if (urlReferrer != null && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("?keywords"))
            {
                return new LinkViewModel { Title = "Results", Url = urlReferrer.OriginalString };
            }

            return new LinkViewModel { Title = "Back to search page", Url = Url.Action("Search", "Standard") };
        }
    }
}