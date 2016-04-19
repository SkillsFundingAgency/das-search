namespace Sfa.Das.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Das.Web.Services;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Web.Extensions;
    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public sealed class ProviderController : Controller
    {
        private readonly IProviderViewModelFactory _viewModelFactory;

        private readonly ILog _logger;

        private readonly IMappingService _mappingService;

        private readonly IProviderSearchService _providerSearchService;

        public ProviderController(
            IProviderSearchService providerSearchService,
            ILog logger,
            IMappingService mappingService,
            IProviderViewModelFactory viewModelFactory)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<ActionResult> StandardResults(ProviderSearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                return RedirectToAction(
                    "Standard",
                    "Apprenticeship",
                    new { id = criteria?.ApprenticeshipId, HasError = true });
            }

            var searchResults =
                await _providerSearchService.SearchByStandardPostCode(criteria.ApprenticeshipId, criteria.PostCode);

            var viewModel =
                _mappingService.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(ProviderSearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                return RedirectToAction(
                    "Framework",
                    "Apprenticeship",
                    new { id = criteria?.ApprenticeshipId, HasError = true });
            }

            var searchResults =
                await _providerSearchService.SearchByFrameworkPostCode(criteria.ApprenticeshipId, criteria.PostCode);

            var viewModel =
                _mappingService.Map<ProviderFrameworkSearchResults, ProviderFrameworkSearchResultViewModel>(
                    searchResults);

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