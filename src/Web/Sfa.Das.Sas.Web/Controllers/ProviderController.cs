using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Extensions;
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

        [HttpGet]
        public async Task<ActionResult> StandardResults(ProviderSearchCriteria criteria)
        {
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

            if (viewModel == null)
            {
                return View((ProviderStandardSearchResultViewModel)null);
            }

            if (viewModel.ResultsToTake != 0)
            {
                viewModel.LastPage = (int)Math.Ceiling(viewModel.TotalResults / viewModel.ResultsToTake);
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

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(ProviderSearchCriteria criteria)
        {
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

            if (viewModel == null)
            {
                return View((ProviderFrameworkSearchResultViewModel)null);
            }

            if (viewModel.ResultsToTake != 0)
            {
                viewModel.LastPage = (int)Math.Ceiling(viewModel.TotalResults / viewModel.ResultsToTake);
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