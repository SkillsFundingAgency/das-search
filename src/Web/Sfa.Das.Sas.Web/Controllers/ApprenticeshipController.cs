using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;
using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System;
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
        private readonly int _nextFundingPeriodMessageCutoffMonths = 3;

        public ApprenticeshipController(ILog logger, IMappingService mappingService, IMediator mediator, IButtonTextService buttonTextService, IFundingBandService fundingBandService)
        {
            _logger = logger;
            _mappingService = mappingService;
            _mediator = mediator;
            _buttonTextService = buttonTextService;
            _fundingBandService = fundingBandService;
        }

        public ActionResult Search()
        {
            return View();
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
        public ActionResult SearchResults(ApprenticeshipSearchQuery query)
        {
            var response = _mediator.Send(query);

            var viewModel = _mappingService.Map<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>(response);

            if (response.StatusCode != ApprenticeshipSearchResponse.ResponseCodes.PageNumberOutOfUpperBound)
            {
                return View(viewModel);
            }

            var rv = CreateRouteParameters(query, viewModel);

            var url = Url.Action("SearchResults", "Apprenticeship", rv);

            return new RedirectResult(url);
        }

        // GET: Standard
        public ActionResult Standard(string id, string keywords)
        {
            _logger.Info($"Getting strandard {id}");
            var response = _mediator.Send(new GetStandardQuery {Id = id, Keywords = keywords});

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
            ProcessViewModel(viewModel, response.Standard?.FundingPeriods, response.Standard?.EffectiveFrom);

            return View(viewModel);
        }
        
        public ActionResult Framework(string id, string keywords)
        {
            _logger.Info($"Getting framework {id}");
            var response = _mediator.Send(new GetFrameworkQuery { Id = id, Keywords = keywords });

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
                    ProcessViewModel(viewModel, response.Framework?.FundingPeriods, response.Framework?.EffectiveFrom);

                    return View(viewModel);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ActionResult SearchForStandardProviders(string standardId, ProviderSearchResponseCodes? statusCode, string postcode, string keywords, string postcodeCountry, bool? isLevyPayingEmployer)
        {
            var query = new GetStandardProvidersQuery
            {
                StandardId = standardId,
                Postcode = postcode,
                Keywords = keywords
            };

            var response = _mediator.Send(query);

            if (response.StatusCode.Equals(GetStandardProvidersResponse.ResponseCodes.NoStandardFound))
            {
                return new HttpNotFoundResult();
            }

            var viewModel = _mappingService.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.PostUrl = Url?.Action("StandardResults", "Provider");
            viewModel.HasError = statusCode.HasValue && statusCode.Value != ProviderSearchResponseCodes.Success;
            viewModel.ErrorMessage = ProviderSearchMapper.CreateErrorMessage(statusCode);
            viewModel.IsLevyPayingEmployer = isLevyPayingEmployer;

            return View("SearchForProviders", viewModel);
        }

        public ActionResult SearchForFrameworkProviders(string frameworkId, ProviderSearchResponseCodes? statusCode, string postcode, string keywords, string postcodeCountry, bool? isLevyPayingEmployer)
        {
            var query = new GetFrameworkProvidersQuery
            {
                FrameworkId = frameworkId,
                Postcode = postcode,
                Keywords = keywords
            };

            var response = _mediator.Send(query);

            if (response.StatusCode.Equals(GetFrameworkProvidersResponse.ResponseCodes.NoFrameworkFound))
            {
                return new HttpNotFoundResult();
            }

            var viewModel = _mappingService.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.PostUrl = Url?.Action("FrameworkResults", "Provider");
            viewModel.HasError = statusCode.HasValue && statusCode.Value != ProviderSearchResponseCodes.Success;
            viewModel.ErrorMessage = ProviderSearchMapper.CreateErrorMessage(statusCode);
            viewModel.IsLevyPayingEmployer = isLevyPayingEmployer;

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

        private void ProcessViewModel(StandardViewModel viewModel, List<FundingPeriod> fundingPeriods, DateTime? effectiveFrom)
        {
            viewModel.FindApprenticeshipTrainingText = _buttonTextService.GetFindTrainingProvidersText(HttpContext);
            var nextFundingCap = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingPeriods, effectiveFrom, _nextFundingPeriodMessageCutoffMonths);
            viewModel.NextEffectiveFrom = nextFundingCap?.EffectiveFrom;
            viewModel.NextFundingCap = nextFundingCap?.FundingCap;
        }

        private void ProcessViewModel(FrameworkViewModel viewModel, List<FundingPeriod> fundingPeriods, DateTime? effectiveFrom)
        {
            viewModel.FindApprenticeshipTrainingText = _buttonTextService.GetFindTrainingProvidersText(HttpContext);
            var nextFundingCap = _fundingBandService.GetNextFundingPeriodWithinTimePeriod(fundingPeriods, effectiveFrom, _nextFundingPeriodMessageCutoffMonths);
            viewModel.NextEffectiveFrom = nextFundingCap?.EffectiveFrom;
            viewModel.NextFundingCap = nextFundingCap?.FundingCap;
        }
    }
}