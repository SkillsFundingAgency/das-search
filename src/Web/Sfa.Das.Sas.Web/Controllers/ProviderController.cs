using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Extensions;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System.Text.RegularExpressions;

    using Sfa.Das.Sas.Core.Configuration;
    using Sfa.Das.Sas.Web.Common;

    public sealed class ProviderController : Controller
    {
        private readonly IProviderViewModelFactory _viewModelFactory;

        private readonly IConfigurationSettings _settings;

        private readonly ILog _logger;

        private readonly IMappingService _mappingService;

        private readonly IProviderSearchService _providerSearchService;

        public ProviderController(
            IProviderSearchService providerSearchService,
            ILog logger,
            IMappingService mappingService,
            IProviderViewModelFactory viewModelFactory,
            IConfigurationSettings settings)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
            _viewModelFactory = viewModelFactory;
            _settings = settings;
        }

        [HttpGet]
        public async Task<ActionResult> StandardResults(ProviderSearchCriteria criteria)
        {
            if (criteria.ApprenticeshipId < 1)
            {
                Response.StatusCode = 400;
            }

            if (string.IsNullOrEmpty(criteria?.PostCode) || !Validation.ValidatePostcode(criteria.PostCode))
            {
                var url = Url.Action(
                    "SearchForProviders",
                    "Apprenticeship",
                    new { HasError = true, standardId = criteria?.ApprenticeshipId });
                return new RedirectResult(url);
            }

            criteria.Page = criteria.Page <= 0 ? 1 : criteria.Page;

            var searchResults =
                await _providerSearchService.SearchByStandardPostCode(criteria.ApprenticeshipId, criteria.PostCode, new Pagination { Page = criteria.Page, Take = criteria.Take }, criteria.DeliveryModes);

            var viewModel =
                _mappingService.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(searchResults);

            if (viewModel == null)
            {
                _logger.Warn("ViewModel is null, StandardResults, ProviderController ");
                return View(new ProviderStandardSearchResultViewModel());
            }

            if (viewModel?.TotalResults > 0 && !viewModel.Hits.Any())
            {
                var url = Url.Action(
                    "StandardResults",
                    "Provider",
                    new { apprenticeshipId = criteria?.ApprenticeshipId, postcode = criteria?.PostCode, page = viewModel.LastPage });
                return new RedirectResult(url);
            }

            if (viewModel.TotalResults <= 0)
            {
                var totalProvidersCountry =
                    await
                        _providerSearchService.SearchByStandard(criteria.ApprenticeshipId, criteria.PostCode, new Pagination {Page = criteria.Page, Take = criteria.Take},
                            criteria.DeliveryModes);
                viewModel.TotalProvidersCountry = totalProvidersCountry.TotalResults;
            }

            viewModel.ActualPage = criteria.Page;

            if (viewModel.StandardNotFound)
            {
                Response.StatusCode = 404;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(ProviderSearchCriteria criteria)
        {
            if (criteria.ApprenticeshipId < 1)
            {
                Response.StatusCode = 400;
            }

            if (string.IsNullOrEmpty(criteria?.PostCode) || !Validation.ValidatePostcode(criteria.PostCode))
            {
                var url = Url.Action(
                    "SearchForProviders",
                    "Apprenticeship",
                    new { HasError = true, frameworkId = criteria?.ApprenticeshipId });
                return new RedirectResult(url);
            }

            criteria.Page = criteria.Page == 0 ? 1 : criteria.Page;

            var searchResults =
                 await _providerSearchService.SearchByFrameworkPostCode(criteria.ApprenticeshipId, criteria.PostCode, new Pagination { Page = criteria.Page, Take = criteria.Take }, criteria.DeliveryModes);

            var viewModel =
                _mappingService.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(searchResults);

            if (viewModel == null)
            {
                _logger.Warn("ViewModel is null, FrameworkResults, ProviderController ");
                return View(new ProviderFrameworkSearchResultViewModel());
            }

            if (viewModel?.TotalResults > 0 && !viewModel.Hits.Any())
            {
                var url = Url.Action(
                    "StandardResults",
                    "Provider",
                    new { apprenticeshipId = criteria?.ApprenticeshipId, postcode = criteria?.PostCode, page = viewModel.LastPage });
                return new RedirectResult(url);
            }

            viewModel.ActualPage = criteria.Page;

            if (viewModel.FrameworkIsMissing)
            {
                Response.StatusCode = 404;
            }

            return View(viewModel);
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

            if (viewModel.Training == ApprenticeshipTrainingType.Standard)
            {
                    viewModel.SearchResultLink =
                        Request.UrlReferrer.GetProviderSearchResultBackUrl(Url.Action("Search", "Apprenticeship"));
            }

            if (viewModel.Training == ApprenticeshipTrainingType.Framework)
            {
                viewModel.SearchResultLink =
                        Request.UrlReferrer.GetProviderSearchResultBackUrl(Url.Action("Search", "Apprenticeship"));
            }

            return View(viewModel);
        }
    }
}