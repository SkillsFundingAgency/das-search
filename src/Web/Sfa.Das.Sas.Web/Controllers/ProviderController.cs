using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class ProviderController : Controller
    {
        private readonly IProviderViewModelFactory _viewModelFactory;

        private readonly IConfigurationSettings _settings;
        private readonly IListCollection<int> _listCollection;

        private readonly IValidation _validation;

        private readonly ILog _logger;

        private readonly IMappingService _mappingService;

        private readonly IProviderSearchService _providerSearchService;

        public ProviderController(
            IProviderSearchService providerSearchService,
            ILog logger,
            IMappingService mappingService,
            IProviderViewModelFactory viewModelFactory,
            IConfigurationSettings settings,
            IListCollection<int> listCollection,
            IValidation validation)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
            _viewModelFactory = viewModelFactory;
            _settings = settings;
            _listCollection = listCollection;
            _validation = validation;
        }

        [HttpGet]
        public async Task<ActionResult> StandardResults(ProviderSearchCriteria criteria)
        {
            if (criteria.ApprenticeshipId < 1)
            {
                return new HttpStatusCodeResult(400);
            }

            if (string.IsNullOrEmpty(criteria?.PostCode) || !_validation.ValidatePostcode(criteria.PostCode))
            {
                var url = Url.Action(
                    "SearchForProviders",
                    "Apprenticeship",
                    new { HasError = true, standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode });
                return new RedirectResult(url);
            }

            criteria.Page = criteria.Page <= 0 ? 1 : criteria.Page;

            var viewModel = await GetStandardSearchResultsViewModel(criteria);

            return await GetView(criteria, viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(ProviderSearchCriteria criteria)
        {
            if (criteria.ApprenticeshipId < 1)
            {
                Response.StatusCode = 400;
            }

            if (string.IsNullOrEmpty(criteria?.PostCode) || !_validation.ValidatePostcode(criteria.PostCode))
            {
                var url = Url.Action(
                    "SearchForProviders",
                    "Apprenticeship",
                    new { HasError = true, frameworkId = criteria?.ApprenticeshipId, postCode = criteria.PostCode });
                return new RedirectResult(url);
            }

            criteria.Page = criteria.Page == 0 ? 1 : criteria.Page;

            var viewModel = await GetFrameworkSearchResultsViewModel(criteria);

            return await GetView(criteria, viewModel);
        }

        [HttpGet]
        public ActionResult Detail(ProviderLocationSearchCriteria criteria)
        {
            var viewModel = _viewModelFactory.GenerateDetailsViewModel(criteria);

            if (viewModel == null)
            {
                var message = $"Cannot find provider: {criteria.ProviderId}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            viewModel.SurveyUrl = _settings.SurveyUrl.ToString();

            viewModel.ProviderId = criteria.ProviderId;

            var cookieListName = viewModel.Training == ApprenticeshipTrainingType.Framework
                ? Constants.FrameworksShortListCookieName
                : Constants.StandardsShortListCookieName;

            var shortlistedApprenticeships = _listCollection.GetAllItems(cookieListName);

            var apprenticeship = shortlistedApprenticeships?.SingleOrDefault(x =>
                    x.ApprenticeshipId.Equals(viewModel.Apprenticeship.Code));

            var isShortlisted = apprenticeship?.ProvidersIdAndLocation.Any(x =>
                x.LocationId.ToString().Equals(criteria.LocationId, StringComparison.Ordinal) &&
                x.ProviderId.Equals(criteria.ProviderId, StringComparison.Ordinal));

            viewModel.IsShortlisted = isShortlisted.HasValue && isShortlisted.Value;

            return View(viewModel);
        }

        private async Task PopulateStandardSearchResultViewModel(ProviderSearchCriteria criteria, ProviderStandardSearchResultViewModel viewModel)
        {
            if (viewModel.TotalResults <= 0)
            {
                var totalProvidersCountry = await _providerSearchService.SearchStandardProviders(
                    criteria.ApprenticeshipId,
                    criteria.PostCode,
                    new Pagination { Page = criteria.Page, Take = criteria.Take },
                    criteria.DeliveryModes,
                    true);

                viewModel.TotalProvidersCountry = totalProvidersCountry.TotalResults;
            }

            viewModel.ActualPage = criteria.Page;
            viewModel.AbsolutePath = Request?.Url?.AbsolutePath;
            viewModel.SearchTerms = criteria.Keywords;
            viewModel.ShowAll = criteria.ShowAll;

            if (viewModel.StandardNotFound)
            {
                Response.StatusCode = 404;
            }
        }

        private async Task PopulateFrameworkSearchResultViewModel(ProviderSearchCriteria criteria, ProviderFrameworkSearchResultViewModel viewModel)
        {
            if (viewModel.TotalResults <= 0)
            {
                var totalProvidersCountry =
                    await
                        _providerSearchService.SearchFrameworkProviders(
                            criteria.ApprenticeshipId,
                            criteria.PostCode,
                            new Pagination { Page = criteria.Page, Take = criteria.Take },
                            criteria.DeliveryModes,
                            true);
                viewModel.TotalProvidersCountry = totalProvidersCountry.TotalResults;
            }

            viewModel.ActualPage = criteria.Page;
            viewModel.AbsolutePath = Request?.Url?.AbsolutePath;
            viewModel.SearchTerms = criteria.Keywords;
            viewModel.ShowAll = criteria.ShowAll;

            if (viewModel.FrameworkIsMissing)
            {
                Response.StatusCode = 404;
            }
        }

        private async Task<ProviderStandardSearchResultViewModel> GetStandardSearchResultsViewModel(ProviderSearchCriteria criteria)
        {
            while (true)
            {
                var searchResults = await _providerSearchService.SearchStandardProviders(
                    criteria.ApprenticeshipId,
                    criteria.PostCode,
                    new Pagination { Page = criteria.Page, Take = criteria.Take },
                    criteria.DeliveryModes,
                    criteria.ShowAll);

                return _mappingService.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(searchResults);
            }
        }

        private async Task<ProviderFrameworkSearchResultViewModel> GetFrameworkSearchResultsViewModel(ProviderSearchCriteria criteria)
        {
            while (true)
            {
                var searchResults = await _providerSearchService.SearchFrameworkProviders(
                    criteria.ApprenticeshipId,
                    criteria.PostCode,
                    new Pagination { Page = criteria.Page, Take = criteria.Take },
                    criteria.DeliveryModes,
                    criteria.ShowAll);

                return _mappingService.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(searchResults);
            }
        }

        private async Task<ActionResult> GetView(ProviderSearchCriteria criteria, ProviderStandardSearchResultViewModel viewModel)
        {
            if (viewModel == null)
            {
                _logger.Warn("ViewModel is null, StandardResults, ProviderController ");
                {
                    return View(new ProviderStandardSearchResultViewModel());
                }
            }

            if (viewModel.TotalResults > 0 && !viewModel.Hits.Any())
            {
                var url = Url.Action(
                    "StandardResults",
                    "Provider",
                    new { apprenticeshipId = criteria?.ApprenticeshipId, postcode = criteria?.PostCode, page = viewModel.LastPage });
                {
                    return new RedirectResult(url);
                }
            }

            await PopulateStandardSearchResultViewModel(criteria, viewModel).ConfigureAwait(false);

            return View(viewModel);
        }

        private async Task<ActionResult> GetView(ProviderSearchCriteria criteria, ProviderFrameworkSearchResultViewModel viewModel)
        {
            if (viewModel == null)
            {
                _logger.Warn("ViewModel is null, FrameworkResults, ProviderController ");
                return View(new ProviderFrameworkSearchResultViewModel());
            }

            if (viewModel?.TotalResults > 0 && !viewModel.Hits.Any())
            {
                var url = Url.Action(
                    "FrameworkResults",
                    "Provider",
                    new { apprenticeshipId = criteria?.ApprenticeshipId, postcode = criteria?.PostCode, page = viewModel.LastPage });
                return new RedirectResult(url);
            }

            await PopulateFrameworkSearchResultViewModel(criteria, viewModel).ConfigureAwait(false);

            return View(viewModel);
        }
    }
}