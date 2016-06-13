﻿using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Extensions;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;
using RedirectResult = System.Web.Mvc.RedirectResult;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System.Net;
    using System.Web.Routing;

    using Sfa.Das.Sas.Web.Factories;
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    public sealed class ApprenticeshipController : Controller
    {
        private readonly ILog _logger;

        private readonly IListCollection<int> _listCollection;

        private readonly IApprenticeshipViewModelFactory _apprenticeshipViewModelFactory;

        private readonly IProfileAStep _profiler;

        private readonly IApprenticeshipSearchService _searchService;

        public ApprenticeshipController(
            IApprenticeshipSearchService searchService,
            ILog logger,
            IProfileAStep profiler,
            IListCollection<int> listCollection,
            IApprenticeshipViewModelFactory apprenticeshipViewModelFactory)
        {
            _searchService = searchService;
            _logger = logger;
            _profiler = profiler;
            _listCollection = listCollection;
            _apprenticeshipViewModelFactory = apprenticeshipViewModelFactory;
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchResults(ApprenticeshipSearchCriteria criteria)
        {
            ApprenticeshipSearchResults searchResults;
            ApprenticeshipSearchResultViewModel viewModel;
            criteria.Page = criteria.Page <= 0 ? 1 : criteria.Page;

            using (_profiler.CreateStep("Search by keyword"))
            {
                searchResults = _searchService.SearchByKeyword(criteria.Keywords, criteria.Page, criteria.Take, criteria.Order, criteria.SelectedLevels);
            }

            using (_profiler.CreateStep("Map to view model"))
            {
                viewModel = _apprenticeshipViewModelFactory.GetSApprenticeshipSearchResultViewModel(searchResults);
            }

            if (viewModel == null)
            {
                _logger.Warn("ViewModel is null, SearchResults, ApprenticeshipController ");
                return View(new ApprenticeshipSearchResultViewModel());
            }

            if (viewModel?.TotalResults > 0 && !viewModel.Results.Any())
            {
                var rv = new RouteValueDictionary { { "keywords", criteria.Keywords }, { "page", viewModel.LastPage } };
                var index = 0;
                foreach (var level in viewModel.AggregationLevel.Where(m => m.Checked))
                {
                    rv.Add("SelectedLevels[" + index + "]", level.Value);
                    index++;
                }

                var url = Url.Action(
                    "SearchResults",
                    "Apprenticeship",
                    rv);
                return new RedirectResult(url);
            }

            viewModel.SortOrder = criteria.Order == 0 ? "1" : criteria.Order.ToString();
            viewModel.ActualPage = criteria.Page;

            return View(viewModel);
        }

        // GET: Standard
        public ActionResult Standard(int id, string keywords)
        {
            if (id < 0)
            {
                Response.StatusCode = 400;
            }

            var viewModel = _apprenticeshipViewModelFactory.GetStandardViewModel(id);
            if (viewModel == null)
            {
                var message = $"Cannot find standard: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var shortlistedApprenticeships = _listCollection.GetAllItems(Constants.StandardsShortListCookieName);
            viewModel.IsShortlisted = shortlistedApprenticeships.Any(x => x.ApprenticeshipId.Equals(id));
            viewModel.SearchTerm = keywords;

            return View(viewModel);
        }

        public ActionResult Framework(int id, string keywords)
        {
            if (id < 0)
            {
                Response.StatusCode = 400;
            }

            var viewModel = _apprenticeshipViewModelFactory.GetFrameworkViewModel(id);
            if (viewModel == null)
            {
                var message = $"Cannot find framework: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var shortlistedApprenticeships = _listCollection.GetAllItems(Constants.FrameworksShortListCookieName);
            viewModel.IsShortlisted = shortlistedApprenticeships.Any(x => x.ApprenticeshipId.Equals(id));
            viewModel.SearchTerm = keywords;

            return View(viewModel);
        }

        public ActionResult SearchForProviders(int? standardId, int? frameworkId, string keywords, string hasError)
        {
            ProviderSearchViewModel viewModel;

            if (standardId != null)
            {
                viewModel = _apprenticeshipViewModelFactory.GetProviderSearchViewModelForStandard(standardId.Value, @Url);
            }
            else if (frameworkId != null)
            {
                viewModel = _apprenticeshipViewModelFactory.GetFrameworkProvidersViewModel(frameworkId.Value, @Url);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }

            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.SearchTerms = keywords;

            return View(viewModel);
        }
    }
}