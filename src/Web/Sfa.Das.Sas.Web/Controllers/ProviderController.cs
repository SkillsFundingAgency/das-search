using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Extensions;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public sealed class ProviderController : Controller
    {
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;
        private readonly IConfigurationSettings _settings;

        public ProviderController(
            ILog logger,
            IMappingService mappingService,
            IMediator mediator,
            IConfigurationSettings settings)
        {
            _logger = logger;
            _mappingService = mappingService;
            _mediator = mediator;
            _settings = settings;
        }

        [HttpGet]
        public async Task<ActionResult> StandardResults(StandardProviderSearchQuery criteria)
        {
            var response = await _mediator.SendAsync(criteria);

            switch (response.StatusCode)
            {
                case ProviderSearchResponseCodes.InvalidApprenticeshipId:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                case ProviderSearchResponseCodes.ApprenticeshipNotFound:
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case ProviderSearchResponseCodes.ServerError:
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                case ProviderSearchResponseCodes.LocationServiceUnavailable:
                case ProviderSearchResponseCodes.PostCodeInvalidFormat:
                case ProviderSearchResponseCodes.WalesPostcode:
                case ProviderSearchResponseCodes.ScotlandPostcode:
                case ProviderSearchResponseCodes.NorthernIrelandPostcode:
                case ProviderSearchResponseCodes.PostCodeTerminated:
                    var postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { standardId = criteria?.ApprenticeshipId, statusCode = response.StatusCode, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);
                case ProviderSearchResponseCodes.PageNumberOutOfUpperBound:
                    var url = Url.Action(
                        "StandardResults",
                        "Provider",
                        GenerateProviderResultsRouteValues(criteria, response.CurrentPage));
                    return new RedirectResult(url);
            }

            var viewModel = _mappingService.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(response, opt => opt
                .AfterMap((src, dest) =>
                {
                    dest.AbsolutePath = Request?.Url?.AbsolutePath;
                    dest.IsLevyPayingEmployer = criteria.IsLevyPayingEmployer;
                    dest.ManageApprenticeshipFunds = new ManageApprenticeshipFundsViewModel(dest.IsLevyPayingEmployer, _settings.ManageApprenticeshipFundsUrl);
                }));

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(FrameworkProviderSearchQuery criteria)
        {
            var response = await _mediator.SendAsync(criteria);

            switch (response.StatusCode)
            {
                case ProviderSearchResponseCodes.InvalidApprenticeshipId:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                case ProviderSearchResponseCodes.ApprenticeshipNotFound:
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case ProviderSearchResponseCodes.ServerError:
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                case ProviderSearchResponseCodes.LocationServiceUnavailable:
                case ProviderSearchResponseCodes.PostCodeInvalidFormat:
                case ProviderSearchResponseCodes.WalesPostcode:
                case ProviderSearchResponseCodes.ScotlandPostcode:
                case ProviderSearchResponseCodes.NorthernIrelandPostcode:
                case ProviderSearchResponseCodes.PostCodeTerminated:
                    var postCodeUrl = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { frameworkId = criteria?.ApprenticeshipId, statusCode = response.StatusCode, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);
                case ProviderSearchResponseCodes.PageNumberOutOfUpperBound:
                    var url = Url.Action(
                        "FrameworkResults",
                        "Provider",
                        GenerateProviderResultsRouteValues(criteria, response.CurrentPage));
                    return new RedirectResult(url);
            }

            var viewModel = _mappingService.Map<FrameworkProviderSearchResponse, ProviderFrameworkSearchResultViewModel>(response, opt => opt
                .AfterMap((src, dest) =>
                {
                    dest.AbsolutePath = Request?.Url?.AbsolutePath;
                    dest.IsLevyPayingEmployer = criteria.IsLevyPayingEmployer;
                    dest.ManageApprenticeshipFunds = new ManageApprenticeshipFundsViewModel(dest.IsLevyPayingEmployer, _settings.ManageApprenticeshipFundsUrl);
                }));

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ProviderDetail(long ukprn, string providerName = "", string pageNumber = "")
        {
            int page;
            if (!int.TryParse(pageNumber, out page))
            {
                page = 1;
            }

            var response = await _mediator.SendAsync(new ProviderDetailQuery { UkPrn = ukprn, Page = page });

            if (response.StatusCode == ProviderDetailResponse.ResponseCodes.ProviderNotFound)
            {
                var message = $"Cannot find provider: {ukprn}";
                _logger.Warn($"404 - {message}");
                return new HttpNotFoundResult(message);
            }

            if (response.StatusCode == ProviderDetailResponse.ResponseCodes.HttpRequestException)
            {
                var message = $"Not able to call the apprenticeship service.";
                _logger.Warn($"{response.StatusCode} - {message}");

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
            }

            var viewModel = ProviderDetailViewModelMapper.GetProviderDetailViewModel(response.Provider, response.ApprenticeshipTrainingSummary);

            return View(viewModel);
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchResults(ProviderNameSearchQuery query)
        {

            var viewModel = new ProviderSearchNameResultViewModel { HasError = false, ShortSearchTerm = false, SearchTerm = query.SearchTerm, TotalResults = 0, Results = null };

            if (query.SearchTerm.Length < 3)
            {
                viewModel.HasError = false;
                viewModel.ShortSearchTerm = true;
                return View(viewModel);
            }

            //var response = _mediator.Send(query);

            //var viewModel = _mappingService.Map<ProviderSearchNameResponse, ProviderSearchNameResultViewModel>(response);

            switch (query.SearchTerm)
            {
                case "error":
                    viewModel.TotalResults = 0;
                    viewModel.HasError = true;
                    viewModel.Results = null;
                    break;
                case "coll":
                    viewModel.TotalResults = 4;
                    viewModel.HasError = false;
                    viewModel.Results = new List<ProviderSearchResultSummary>
                    {
                        new ProviderSearchResultSummary { ProviderName = "Abingdon and Witney College", UkPrn = 10000055 },
                        new ProviderSearchResultSummary { ProviderName = "Accrington and Rossendale College", UkPrn = 10000093 },
                        new ProviderSearchResultSummary { ProviderName = "Andrew Collinge Training Limited", UkPrn = 10000285 },
                        new ProviderSearchResultSummary
                        {
                            ProviderName = "NCG", UkPrn = 10004599, Aliases = new List<string> { "Newcastle College", "Kidderminster College", "Newcastle Sixth Form College", "West Lancashire College", "Carlisle College", "Lewisham Southwark College" } }
                    };

                    break;
                case "pages":
                    viewModel.TotalResults = 21;
                    viewModel.HasError = false;
                    viewModel.LastPage = 2;
                    viewModel.ActualPage = query.Page;
                    if (query.Page == 1)
                    {

                        viewModel.Results = new List<ProviderSearchResultSummary>
                        {
                            new ProviderSearchResultSummary {ProviderName = "ABC01", UkPrn = 10000055},
                            new ProviderSearchResultSummary {ProviderName = "ABC02", UkPrn = 10000093},
                            new ProviderSearchResultSummary {ProviderName = "ABC03", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC04", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC05", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC06", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC07", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC08", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC09", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC10", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC11", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC12", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC13", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC14", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC15", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC16", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC17", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC18", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC19", UkPrn = 10000285},
                            new ProviderSearchResultSummary {ProviderName = "ABC20", UkPrn = 10000285}
                        };
                    }

                    if (query.Page == 2)
                    {
                        viewModel.Results = new List<ProviderSearchResultSummary>
                        {
                            new ProviderSearchResultSummary {ProviderName = "ABC21", UkPrn = 10000055}
                        };
                    }

                    break;
                case "zero":
                    viewModel.TotalResults = 0;
                    viewModel.HasError = false;
                    viewModel.Results = null;
                    break;
                default:
                    viewModel.TotalResults = 0;
                    viewModel.Results = null;
                    viewModel.HasError = false;
                    break;
            }

            return View(viewModel);
        }


        [HttpGet]
        public ActionResult Detail(ApprenticeshipProviderDetailQuery criteria)
        {
            var response = _mediator.Send(criteria);

            switch (response.StatusCode)
            {
                case ApprenticeshipProviderDetailResponse.ResponseCodes.ApprenticeshipProviderNotFound:
                    _logger.Warn($"404 - Cannot find provider: ({criteria.UkPrn}) for apprenticeship product: ({criteria.FrameworkId ?? criteria.StandardCode}) with location: ({criteria.LocationId})");
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case ApprenticeshipProviderDetailResponse.ResponseCodes.InvalidInput:
                    _logger.Warn($"400 - Bad Request: {criteria.UkPrn}");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = _mappingService.Map<ApprenticeshipProviderDetailResponse, ApprenticeshipDetailsViewModel>(response, opt => opt
                .AfterMap((src, dest) =>
                {
                    dest.SurveyUrl = _settings.SurveyUrl.ToString();
                    dest.SatisfactionSourceUrl = _settings.SatisfactionSourceUrl.ToString();
                    dest.AchievementRateSourceUrl = _settings.AchievementRateUrl.ToString();
                    dest.IsLevyPayingEmployer = criteria.IsLevyPayingEmployer;
                    dest.ManageApprenticeshipFunds = new ManageApprenticeshipFundsViewModel(dest.IsLevyPayingEmployer, _settings.ManageApprenticeshipFundsUrl);
                }));

            return View(viewModel);
        }

        private static RouteValueDictionary GenerateProviderResultsRouteValues(ProviderSearchQuery criteria, int currentPage)
        {
            return new RouteValueDictionary()
                .AddValue("page", currentPage)
                .AddValue("postcode", criteria?.PostCode ?? string.Empty)
                .AddValue("apprenticeshipId", criteria?.ApprenticeshipId)
                .AddValue("showall", criteria?.ShowAll)
                .AddValue("isLevyPayingEmployer", criteria?.IsLevyPayingEmployer)
                .AddList("deliverymodes", criteria?.DeliveryModes);
        }
    }
}