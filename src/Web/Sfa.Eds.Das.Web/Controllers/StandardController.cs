namespace Sfa.Eds.Das.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Core.Domain.Services;
    using Core.Logging;
    using Models;
    using Services;
    using ViewModels;

    public sealed class StandardController : Controller
    {
        private readonly IStandardSearchService _searchService;

        private readonly IStandardRepository _standardRepository;

        private readonly ILog _logger;
        private readonly IMappingService _mappingService;

        public StandardController(IStandardSearchService searchService, IStandardRepository standardRepository, ILog logger, IMappingService mappingService)
        {
            _searchService = searchService;
            _standardRepository = standardRepository;
            _logger = logger;
            _mappingService = mappingService;
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchResults(StandardSearchCriteria criteria)
        {
            var searchResults = _searchService.SearchByKeyword(criteria.Keywords, criteria.Skip, criteria.Take);

            var viewModel = _mappingService.Map<StandardSearchResults, StandardSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        // GET: Standard
        public ActionResult Detail(string id, string hasError)
        {
            var standardResult = this._standardRepository.GetById(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var error = false;
            if (!string.IsNullOrEmpty(hasError))
            {
                error = bool.Parse(hasError);
            }

            var standardDetail = new StandardDetail
            {
                Standard = standardResult,
                HasError = error
            };

            var viewModel = _mappingService.Map<StandardDetail, StandardViewModel>(standardDetail);
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