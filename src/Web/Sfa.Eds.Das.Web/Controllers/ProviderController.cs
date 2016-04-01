using System.Threading.Tasks;
using System.Web.Mvc;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
﻿using Sfa.Eds.Das.Web.Extensions;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;
using Sfa.Eds.Das.Web.ViewModels;

namespace Sfa.Eds.Das.Web.Controllers
{
    public sealed class ProviderController : Controller
    {
        private readonly IProviderSearchService _providerSearchService;
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;

        public ProviderController(
            IProviderSearchService providerSearchService,
            ILog logger,
            IMappingService mappingService,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
        }

        [HttpGet]
        public async Task<ActionResult> StandardResults(ProviderSearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                return RedirectToAction("Standard", "Apprenticeship", new { id = criteria.StandardId, HasError = true });
            }

            var searchResults = await _providerSearchService.SearchByPostCode(criteria.StandardId, criteria.PostCode);

            var viewModel = _mappingService.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(ProviderSearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                return RedirectToAction("Framework", "Apprenticeship", new { id = criteria.StandardId, HasError = true });
            }

            var searchResults = await _providerSearchService.SearchByPostCode(criteria.StandardId, criteria.PostCode);

            var viewModel = _mappingService.Map<ProviderStandardSearchResults, ProviderStandardSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Detail(ProviderLocationSearchCriteria criteria)
        {
            var model = _apprenticeshipProviderRepository.GetById(criteria.ProviderId, criteria.LocationId, criteria.StandardCode);

            if (model == null)
            {
                var message = $"Cannot find provider: {criteria.ProviderId}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var viewModel = _mappingService.Map<Provider, ProviderViewModel>(model);

            var apprenticeshipData = _getStandards.GetStandardById(model.Apprenticeship.Code);

            viewModel.ApprenticeshipNameWithLevel = string.Concat(apprenticeshipData.Title, " level ", apprenticeshipData.NotionalEndLevel);

            viewModel.SearchResultLink = Request.UrlReferrer.GetProviderSearchResultUrl(Url.Action("StandardResults", "Provider"));

            return View(viewModel);
        }
    }
}