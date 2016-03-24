using System.Threading.Tasks;
using System.Web.Mvc;
using Sfa.Das.ApplicationServices;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Eds.Das.Web.Models;
using Sfa.Eds.Das.Web.Services;
using Sfa.Eds.Das.Web.ViewModels;
﻿using Sfa.Eds.Das.Web.Extensions;

namespace Sfa.Eds.Das.Web.Controllers
{
    public sealed class ProviderController : Controller
    {
        private readonly IProviderSearchService _providerSearchService;
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        private readonly IStandardRepository _standardRepository;

        public ProviderController(
            IProviderSearchService providerSearchService,
            ILog logger,
            IMappingService mappingService,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository,
            IStandardRepository standardRepository)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
            _standardRepository = standardRepository;
        }

        [HttpGet]
        public async Task<ActionResult> SearchResults(ProviderSearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                return RedirectToAction("Detail", "Standard", new { id = criteria.StandardId, HasError = true });
            }

            var searchResults = await _providerSearchService.SearchByPostCode(criteria.StandardId, criteria.PostCode);

            var viewModel = _mappingService.Map<ProviderSearchResults, ProviderSearchResultViewModel>(searchResults);

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

            var apprenticeshipData = _standardRepository.GetById(model.Apprenticeship.Code);

            viewModel.ApprenticeshipNameWithLevel = string.Concat(apprenticeshipData.Title, " level ", apprenticeshipData.NotionalEndLevel);

            viewModel.SearchResultLink = Request.UrlReferrer.GetProviderSearchResultUrl(Url.Action("SearchResults", "Provider"));

            return View(viewModel);
        }
    }
}