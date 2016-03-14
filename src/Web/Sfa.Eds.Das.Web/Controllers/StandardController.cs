namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;

    using Core.Domain.Services;
    using Core.Logging;
    using Models;
    using Services;
    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Web.Extensions;
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

            var viewModel = _mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        // GET: Standard
        public ActionResult Detail(int id, string hasError)
        {
            var standardResult = _standardRepository.GetById(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var viewModel = _mappingService.Map<Standard, StandardViewModel>(standardResult);

            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.SearchResultLink = Request.UrlReferrer.GetSearchResultUrl(Url.Action("Search", "Standard"));

            return View(viewModel);
        }

        public ActionResult FrameworkDetail(int id)
        {
            var resultUrl = Request.UrlReferrer.GetSearchResultUrl(Url.Action("Search", "Standard"));
            return Redirect(resultUrl.Url);
        }
    }
}