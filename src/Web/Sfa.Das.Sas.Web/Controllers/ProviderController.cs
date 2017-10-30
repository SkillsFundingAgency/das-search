using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using Core.Configuration;
    using Extensions;
    using MediatR;
    using Services;
    using SFA.DAS.NLog.Logger;
    using ViewModels;

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
            string postCodeUrl;

            StandardProviderSearchResponse response;

            // MFCMFC
            if (criteria.PostCode == "CV1 2WT")
            {
                response = new StandardProviderSearchResponse {StatusCode = StandardProviderSearchResponse.ResponseCodes.Success};
            }
            else
            {
                response = await _mediator.SendAsync(criteria);
            }

            switch (response.StatusCode)
            {
                case StandardProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                case StandardProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound:
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case StandardProviderSearchResponse.ResponseCodes.ServerError:
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                case StandardProviderSearchResponse.ResponseCodes.LocationServiceUnavailable:
                    postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { HasError = true, standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat:
                    postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { WrongPostcode = true, standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.WalesPostcode:
                    postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { PostcodeCountry = "Wales", standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.ScotlandPostcode:
                    postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { PostcodeCountry = "Scotland", standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.NorthernIrelandPostcode:
                    postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { PostcodeCountry = "NorthernIreland", standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound:
                    var url = Url.Action(
                        "StandardResults",
                        "Provider",
                        GenerateProviderResultsRouteValues(criteria, response.CurrentPage));
                    return new RedirectResult(url);
            }

            ProviderStandardSearchResultViewModel viewModel;

            if (criteria.PostCode == "CV1 2WT")
            {
                viewModel = new ProviderStandardSearchResultViewModel
                {
                    TotalResults = 1,
                    ResultsToTake = 1,
                    ActualPage = 1,
                    LastPage = 1,
                    StandardId = "91",
                    StandardName = "software standard",
                    StandardLevel = null,
                    PostCode = criteria.PostCode,
                    Hits = new List<StandardProviderResultItemViewModel>
                    {
                        new StandardProviderResultItemViewModel
                        {
                            AchievementRateMessage = "achievement rate message",
                            Address = new Address
                            {
                                Address1 = "123 Brick Lane"

                            },
                            ContactUsUrl = "http://contactus",
                            DeliveryModes = new List<string> { "DayRelease", "BlockRelease", "100PercentEmployer" },
                            UkPrn = 1,
                            LocationId = 1,
                            StandardCode = 1,
                            ProviderName = "name 1"
                        }
                    },
                    PostCodeMissing = false,
                    TotalResultsForCountry = 30,
                    DeliveryModes = new List<DeliveryModeViewModel>
                                        {
                        new DeliveryModeViewModel { Checked = false, Value = "DayRelease", Title = "day release", Count = 25 },
                        new DeliveryModeViewModel { Checked = true, Value = "BlockRelease", Title = "block release", Count = 28 },
                        new DeliveryModeViewModel { Checked = true, Value = "100PercentLocation", Title = "at your location", Count = 30 }
                        },
                    NationalProviders = new NationalProviderViewModel { Checked = false, Count = 80, Title = "national providers", Value = "NationalProviders" },
                    SearchTerms = "software",
                    AbsolutePath = "aboslutepath",
                    ShowAll = false,
                    ShowNationalProviders = false,
                    IsLevyPayingEmployer = true,
                    
                };
            }
            else
            {
                viewModel = _mappingService.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(response, opt => opt
                    .AfterMap((src, dest) =>
                    {
                        dest.AbsolutePath = Request?.Url?.AbsolutePath;
                        dest.IsLevyPayingEmployer = criteria.IsLevyPayingEmployer;
                    }));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(FrameworkProviderSearchQuery criteria)
        {
            string url;

            var response = await _mediator.SendAsync(criteria);

            switch (response.StatusCode)
            {
                case FrameworkProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                case FrameworkProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound:
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case FrameworkProviderSearchResponse.ResponseCodes.ServerError:
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

                case FrameworkProviderSearchResponse.ResponseCodes.LocationServiceUnavailable:
                    url = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { HasError = true, frameworkId = criteria?.ApprenticeshipId, postCode = criteria.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat:
                    url = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { WrongPostcode = true, frameworkId = criteria?.ApprenticeshipId, postCode = criteria?.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.WalesPostcode:
                    url = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { PostcodeCountry = "Wales", frameworkId = criteria?.ApprenticeshipId, postCode = criteria?.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.ScotlandPostcode:
                    url = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { PostcodeCountry = "Scotland", frameworkId = criteria?.ApprenticeshipId, postCode = criteria?.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.NorthernIrelandPostcode:
                    url = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { PostcodeCountry = "NorthernIreland", frameworkId = criteria?.ApprenticeshipId, postCode = criteria?.PostCode, isLevyPayingEmployer = criteria.IsLevyPayingEmployer });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound:
                    url = Url.Action(
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
                }));

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ProviderDetail(long ukprn, string providerName = "")
        {
            var response = await _mediator.SendAsync(new ProviderDetailQuery { UkPrn = ukprn });

            if (response.StatusCode == ProviderDetailResponse.ResponseCodes.ProviderNotFound)
            {
                var message = $"Cannot find provider: {ukprn}";
                _logger.Warn($"404 - {message}");
                return new HttpNotFoundResult(message);
            }

            if (response.StatusCode == ProviderDetailResponse.ResponseCodes.HttpRequestException)
            {
                var message = $"Provider Id wrong length: {ukprn}";
                _logger.Warn($"400 - {message}");

                return new HttpNotFoundResult(message);
            }

            var viewModel = ProviderDetailViewModelMapper.GetProviderDetailViewModel(response.Provider);
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