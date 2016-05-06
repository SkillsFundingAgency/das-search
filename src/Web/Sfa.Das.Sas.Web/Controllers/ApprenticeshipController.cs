using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Extensions;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Repositories;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class ApprenticeshipController : Controller
    {
        public const string StandardsShortListCookieName = "standards_shortlist";

        private readonly ILog _logger;

        private readonly IMappingService _mappingService;
        private readonly IWebStore<int> _webStore;

        private readonly IProfileAStep _profiler;

        private readonly IApprenticeshipSearchService _searchService;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        public ApprenticeshipController(
            IApprenticeshipSearchService searchService,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            ILog logger,
            IMappingService mappingService,
            IProfileAStep profiler)
            IMappingService mappingService, 
            IWebStore<int> webStore)
        {
            _searchService = searchService;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _logger = logger;
            _mappingService = mappingService;
            _profiler = profiler;
            _webStore = webStore;
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchResults(StandardSearchCriteria criteria)
        {
            ApprenticeshipSearchResults searchResults;
            ApprenticeshipSearchResultViewModel viewModel;

            using (_profiler.CreateStep("Search by keyword"))
            {
                searchResults = _searchService.SearchByKeyword(criteria.Keywords, criteria.Skip, criteria.Take);
            }

            using (_profiler.CreateStep("Map to view model"))
            {
                viewModel = _mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(searchResults);
            }

            return View(viewModel);
        }

        // GET: Standard
        public ActionResult Standard(int id, string hasError)
        {
            var standardResult = _getStandards.GetStandardById(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var shortListStandards = _webStore.FindAllItems(StandardsShortListCookieName);
            
            var viewModel = _mappingService.Map<Standard, StandardViewModel>(standardResult);

            viewModel.IsShortListed = shortListStandards.Contains(id);
            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.SearchResultLink = Request.UrlReferrer.GetSearchResultUrl(Url.Action("Search", "Apprenticeship"));

            return View(viewModel);
        }

        public ActionResult Framework(int id, string hasError)
        {
            var frameworkResult = _getFrameworks.GetFrameworkById(id);

            if (frameworkResult == null)
            {
                var message = $"Cannot find framework: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var viewModel = _mappingService.Map<Framework, FrameworkViewModel>(frameworkResult);

            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.SearchResultLink = Request.UrlReferrer.GetSearchResultUrl(Url.Action("Search", "Apprenticeship"));

            return View(viewModel);
        }

        public ActionResult StandardShortList(int id, string listAction)
        {
            var standardResult = _getStandards.GetStandardById(id);

            if (standardResult == null || string.IsNullOrEmpty(listAction))
            {
                return RedirectToAction("Standard", new { id = id });
            }

            if (listAction.Equals("save", StringComparison.CurrentCultureIgnoreCase))
            {
                _webStore.AddItem(StandardsShortListCookieName, id);
            }
            else if (listAction.Equals("remove", StringComparison.CurrentCultureIgnoreCase))
            {
                _webStore.RemoveItem(StandardsShortListCookieName, id);
            }

            return RedirectToAction("Standard", new { id=id });
        }
    }
}