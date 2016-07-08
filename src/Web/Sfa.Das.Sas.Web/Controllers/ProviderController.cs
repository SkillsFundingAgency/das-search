using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System.Net;
    using System.Web.Routing;
    using Sfa.Das.Sas.Web.Extensions;

    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public sealed class ProviderController : Controller
    {
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;

        public ProviderController(
            ILog logger,
            IMappingService mappingService,
            IMediator mediator)
        {
            _logger = logger;
            _mappingService = mappingService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> StandardResults(StandardProviderSearchQuery criteria)
        {
            var response = await _mediator.SendAsync(criteria);

            switch (response.StatusCode)
            {
                case StandardProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                case StandardProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound:
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat:
                    var postCodeUrl = Url.Action(
                        "SearchForProviders",
                        "Apprenticeship",
                        new { HasError = true, standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound:

                    var url = Url.Action(
                        "StandardResults",
                        "Provider",
                        GenerateStandardProviderResultsRouteValues(criteria, response));

                    return new RedirectResult(url);
            }

            var viewModel = _mappingService.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(response, opt => opt
                                .AfterMap((src, dest) => dest.AbsolutePath = Request?.Url?.AbsolutePath));

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> FrameworkResults(FrameworkProviderSearchQuery criteria)
        {
            var response = await _mediator.SendAsync(criteria);

            switch (response.StatusCode)
            {
                case FrameworkProviderSearchResponse.ResponseCodes.InvalidApprenticeshipId:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                case FrameworkProviderSearchResponse.ResponseCodes.ApprenticeshipNotFound:
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat:
                    var urlPostCodeSearch = Url.Action(
                        "SearchForProviders",
                        "Apprenticeship",
                        new { HasError = true, frameworkId = criteria?.ApprenticeshipId, postCode = criteria?.PostCode });
                    return new RedirectResult(urlPostCodeSearch);

                case FrameworkProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound:
                    var url = Url.Action(
                        "FrameworkResults",
                        "Provider",
                        GenerateFrameworkProviderResultsRouteValues(criteria, response));

                    return new RedirectResult(url);
            }

            var viewModel = _mappingService.Map<FrameworkProviderSearchResponse, ProviderFrameworkSearchResultViewModel>(response, opt => opt
                                .AfterMap((src, dest) => dest.AbsolutePath = Request?.Url?.AbsolutePath));

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Detail(ProviderDetailQuery criteria)
        {
            var response = _mediator.Send(criteria);

            switch (response.StatusCode)
            {
                case DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound:
                    _logger.Warn($"404 - Cannot find provider: {criteria.ProviderId}");
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case DetailProviderResponse.ResponseCodes.InvalidInput:
                    _logger.Warn($"400 - Bad Request: {criteria.ProviderId}");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = _mappingService.Map<DetailProviderResponse, ApprenticeshipDetailsViewModel>(response);

            return View(viewModel);
        }

        private static RouteValueDictionary GenerateStandardProviderResultsRouteValues(StandardProviderSearchQuery criteria, StandardProviderSearchResponse response)
        {
            return new RouteValueDictionary()
                        .AddValue("page", response.CurrentPage)
                        .AddValue("postcode", criteria?.PostCode ?? string.Empty)
                        .AddValue("apprenticeshipId", criteria?.ApprenticeshipId)
                        .AddList("deliverymodes", criteria?.DeliveryModes);
        }

        private static RouteValueDictionary GenerateFrameworkProviderResultsRouteValues(FrameworkProviderSearchQuery criteria, FrameworkProviderSearchResponse response)
        {
            return new RouteValueDictionary()
                        .AddValue("page", response.CurrentPage)
                        .AddValue("postcode", criteria?.PostCode ?? string.Empty)
                        .AddValue("apprenticeshipId", criteria?.ApprenticeshipId)
                        .AddList("deliverymodes", criteria?.DeliveryModes);
        }
    }
}
