﻿using System.Threading.Tasks;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using Attribute;
    using MediatR;
    using Services;
    using Services.MappingActions.Helpers;
    using Sfa.Das.Sas.Core.Configuration;
    using SFA.DAS.Apprenticeships.Api.Types;
    using SFA.DAS.NLog.Logger;
    using ViewModels;

    [NoCache]
    public sealed class ApprenticeshipController : Controller
    {
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;
        private readonly IButtonTextService _buttonTextService;
        private readonly IFundingBandService _fundingBandService;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly int _nextFundingPeriodMessageCutoffMonths = 3;

        public ApprenticeshipController(
            ILog logger,
            IMappingService mappingService,
            IMediator mediator,
            IButtonTextService buttonTextService,
            IFundingBandService fundingBandService,
            IConfigurationSettings configurationSettings)
        {
            _logger = logger;
            _mappingService = mappingService;
            _mediator = mediator;
            _buttonTextService = buttonTextService;
            _fundingBandService = fundingBandService;
            _configurationSettings = configurationSettings;
        }

        public ActionResult Search()
        {
            return View(new ApprenticeshipSearchViewModel { ApprenticeshipInfoApiBaseUrl = _configurationSettings.ApprenticeshipApiBaseUrl });
        }

        public ActionResult ApprenticeshipOrProvider(bool? retry, bool? isApprenticeship)
        {

            var viewModel = new ApprenticeshipOrProviderViewModel();

            if (retry != null && isApprenticeship == null)
            {
                viewModel.HasError = true;
                return View(viewModel);
            }

            switch (isApprenticeship)
            {
                case null:
                    return View(viewModel);
                case true:
                    {
                        var url = Url.Action("Search", "Apprenticeship");
                        return new RedirectResult(url);
                    }
                default:
                    {
                        var url = Url.Action("Search", "Provider");
                        return new RedirectResult(url);
                    }
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchResults(ApprenticeshipSearchQuery query)
        {
            var response = await _mediator.Send(query);

            var viewModel = _mappingService.Map<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>(response);
            viewModel.SearchViewModel = new ApprenticeshipSearchViewModel
            {
                ApprenticeshipInfoApiBaseUrl = _configurationSettings.ApprenticeshipApiBaseUrl,
                SearchTerm = viewModel.SearchTerm
            };

            if (response.StatusCode != ApprenticeshipSearchResponse.ResponseCodes.PageNumberOutOfUpperBound)
            {
                return View(viewModel);
            }

            var rv = CreateRouteParameters(query, viewModel);

            var url = Url.Action("SearchResults", "Apprenticeship", rv);

            return new RedirectResult(url);
        }

        // GET: Standard
        public async Task<ActionResult> Standard(string id, string keyword, string ukprn = null)
        {
            _logger.Info($"Getting standard {id}");
            var response = await _mediator.Send(new GetStandardQuery { Id = id, Keywords = keyword });

            string message;

            switch (response.StatusCode)
            {
                case GetStandardResponse.ResponseCodes.InvalidStandardId:
                    {
                        _logger.Info("404 - Attempt to get standard with an ID below zero");
                        return HttpNotFound("Cannot find any standards with an ID below zero");
                    }

                case GetStandardResponse.ResponseCodes.StandardNotFound:
                    {
                        message = $"Cannot find standard: {id}";
                        _logger.Warn($"404 - {message}");

                        return new HttpNotFoundResult(message);
                    }

                case GetStandardResponse.ResponseCodes.AssessmentOrgsEntityNotFound:
                    {
                        message = $"Cannot find assessment organisations for standard: {id}";
                        _logger.Warn($"404 - {message}");
                        break;
                    }

                case GetStandardResponse.ResponseCodes.Gone:
                    {
                        message = $"Expired standard request: {id}";

                        _logger.Warn($"410 - {message}");

                        return new HttpStatusCodeResult(HttpStatusCode.Gone);
                    }

                case GetStandardResponse.ResponseCodes.HttpRequestException:
                    {
                        message = $"Request error when requesting assessment orgs for standard: {id}";
                        _logger.Warn($"400 - {message}");

                        return new HttpNotFoundResult(message);
                    }
            }

            _logger.Info($"Mapping Standard {id}");
            var viewModel = _mappingService.Map<GetStandardResponse, StandardViewModel>(response);
            ProcessViewModel(viewModel, response.Standard?.FundingPeriods, response.Standard?.EffectiveFrom, ukprn);

            return View(viewModel);
        }
        
        public async Task<ActionResult> Framework(string id, string keyword, string ukprn = null)
        {
            _logger.Info($"Getting framework {id}");
            var response = await _mediator.Send(new GetFrameworkQuery { Id = id, Keywords = keyword });

            string message;

            switch (response.StatusCode)
            {
                case GetFrameworkResponse.ResponseCodes.InvalidFrameworkId:
                    _logger.Info("404 - Framework id has wrong format");

                    return HttpNotFound("Framework id has wrong format");

                case GetFrameworkResponse.ResponseCodes.FrameworkNotFound:
                    message = $"Cannot find framework: {id}";

                    _logger.Warn($"404 - {message}");

                    return new HttpNotFoundResult(message);

                case GetFrameworkResponse.ResponseCodes.Gone:
                    message = $"Expired framework request: {id}";

                    _logger.Warn($"410 - {message}");

                    return new HttpStatusCodeResult(HttpStatusCode.Gone);

                case GetFrameworkResponse.ResponseCodes.Success:
                    _logger.Info($"Mapping Framework {id}");
                    var viewModel = _mappingService.Map<GetFrameworkResponse, FrameworkViewModel>(response);
                    ProcessViewModel(viewModel, response.Framework?.FundingPeriods, response.Framework?.EffectiveFrom, ukprn);

                    return View(viewModel);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<ActionResult> SearchForStandardProviders(string standardId, ProviderSearchResponseCodes? statusCode, string postcode, string keywords,string ukprn, string postcodeCountry, bool? isLevyPayingEmployer)
        {
            var query = new GetStandardProvidersQuery
            {
                StandardId = standardId,
                Postcode = postcode,
                Keywords = keywords
            };

            var response = await _mediator.Send(query);

            if (response.StatusCode.Equals(GetStandardProvidersResponse.ResponseCodes.NoStandardFound))
            {
                return new HttpNotFoundResult();
            }

            var viewModel = _mappingService.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.PostUrl = Url?.Action("StandardResults", "Provider");
            viewModel.HasError = statusCode.HasValue && statusCode.Value != ProviderSearchResponseCodes.Success;
            viewModel.ErrorMessage = ProviderSearchMapper.CreateErrorMessage(statusCode);
            viewModel.IsLevyPayingEmployer = isLevyPayingEmployer;
            viewModel.Ukprn = ukprn;

            return View("SearchForProviders", viewModel);
        }

        public async Task<ActionResult> SearchForFrameworkProviders(string frameworkId, ProviderSearchResponseCodes? statusCode, string postcode, string keywords, string ukprn, string postcodeCountry, bool? isLevyPayingEmployer)
        {
            var query = new GetFrameworkProvidersQuery
            {
                FrameworkId = frameworkId,
                Postcode = postcode,
                Keywords = keywords
            };

            var response = await _mediator.Send(query);

            if (response.StatusCode.Equals(GetFrameworkProvidersResponse.ResponseCodes.NoFrameworkFound))
            {
                return new HttpNotFoundResult();
            }

            var viewModel = _mappingService.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.PostUrl = Url?.Action("FrameworkResults", "Provider");
            viewModel.HasError = statusCode.HasValue && statusCode.Value != ProviderSearchResponseCodes.Success;
            viewModel.ErrorMessage = ProviderSearchMapper.CreateErrorMessage(statusCode);
            viewModel.IsLevyPayingEmployer = isLevyPayingEmployer;
            viewModel.Ukprn = ukprn;

            return View("SearchForProviders", viewModel);
        }

        private static RouteValueDictionary CreateRouteParameters(ApprenticeshipSearchQuery query, ApprenticeshipSearchResultViewModel viewModel)
        {
            var rv = new RouteValueDictionary { { "keywords", query?.Keywords }, { "page", viewModel?.LastPage ?? 1 } };
            var index = 0;

            if (viewModel?.AggregationLevel == null || !viewModel.AggregationLevel.Any())
            {
                return rv;
            }

            foreach (var level in viewModel.AggregationLevel.Where(m => m.Checked))
            {
                rv.Add("SelectedLevels[" + index + "]", level.Value);
                index++;
            }

            return rv;
        }

        private void ProcessViewModel(StandardViewModel viewModel, List<FundingPeriod> fundingPeriods, DateTime? effectiveFrom, string ukprn)
        {
            viewModel.FindApprenticeshipTrainingText = _buttonTextService.GetFindTrainingProvidersText(HttpContext);
            var nextFundingCap = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingPeriods, effectiveFrom, _nextFundingPeriodMessageCutoffMonths);
            viewModel.NextEffectiveFrom = nextFundingCap?.EffectiveFrom;
            viewModel.NextFundingCap = nextFundingCap?.FundingCap;
            viewModel.Ukprn = ukprn;
        }

        private void ProcessViewModel(FrameworkViewModel viewModel, List<FundingPeriod> fundingPeriods, DateTime? effectiveFrom, string ukprn)
        {
            viewModel.FindApprenticeshipTrainingText = _buttonTextService.GetFindTrainingProvidersText(HttpContext);
            var nextFundingCap = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingPeriods, effectiveFrom, _nextFundingPeriodMessageCutoffMonths);
            viewModel.NextEffectiveFrom = nextFundingCap?.EffectiveFrom;
            viewModel.NextFundingCap = nextFundingCap?.FundingCap;
            viewModel.Ukprn = ukprn;
        }
    }
}