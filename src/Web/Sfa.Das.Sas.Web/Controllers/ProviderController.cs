﻿using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
    using Sfa.Das.Sas.Core.Configuration;

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

        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> StandardResults(ProviderSearchCriteria criteria)
        {
            if (criteria.ApprenticeshipId < 1)
            {
                Response.StatusCode = 400;
            }

            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                var url = Url.Action(
                    "Standard",
                    "Apprenticeship",
                    new { id = criteria?.ApprenticeshipId, HasError = true });
                var anchor = string.IsNullOrEmpty(criteria?.InputId) ? string.Empty : $"#{criteria.InputId}";
                return new RedirectResult($"{url}{anchor}");
            }

            criteria.Page = criteria.Page == 0 ? 1 : criteria.Page;

            var searchResults =
                await _providerSearchService.SearchByStandardPostCode(criteria.ApprenticeshipId, criteria.PostCode, criteria.Page, criteria.Take, criteria.DeliveryModes);

            var viewModel =
                _mappingService.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(searchResults);

            if (viewModel.StandardNotFound)
            {
                Response.StatusCode = 404;
            }

            return View(viewModel);
        }

        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> FrameworkResults(ProviderSearchCriteria criteria)
        {
            if (criteria.ApprenticeshipId < 1)
            {
                Response.StatusCode = 400;
            }

            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                var url = Url.Action(
                    "Framework",
                    "Apprenticeship",
                    new { id = criteria?.ApprenticeshipId, HasError = true });
                var anchor = string.IsNullOrEmpty(criteria?.InputId) ? string.Empty : $"#{criteria.InputId}";
                return new RedirectResult($"{url}{anchor}");
            }

            criteria.Page = criteria.Page == 0 ? 1 : criteria.Page;

            var searchResults =
                 await _providerSearchService.SearchByFrameworkPostCode(criteria.ApprenticeshipId, criteria.PostCode, criteria.Page, criteria.Take, criteria.DeliveryModes);

            var viewModel =
                _mappingService.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(searchResults);

            if (viewModel.FrameworkIsMissing)
            {
                Response.StatusCode = 404;
            }

            return View(viewModel);
        }

        [System.Web.Mvc.HttpGet]
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