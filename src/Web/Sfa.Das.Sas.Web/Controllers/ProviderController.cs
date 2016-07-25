using System;
﻿using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
﻿using Sfa.Das.Sas.Core.Configuration;
﻿using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Extensions;
using Sfa.Das.Sas.Web.Services;
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
            string postCodeUrl = string.Empty;
            var response = await _mediator.SendAsync(criteria);

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
                        new {HasError = true, standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode});
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat:
                    postCodeUrl = Url.Action(
                        "SearchForStandardProviders",
                        "Apprenticeship",
                        new { WrongPostcode = true, standardId = criteria?.ApprenticeshipId, postCode = criteria.PostCode });
                    return new RedirectResult(postCodeUrl);

                case StandardProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound:
                    var url = Url.Action(
                        "StandardResults",
                        "Provider",
                        GenerateProviderResultsRouteValues(criteria, response.CurrentPage));
                    return new RedirectResult(url);
            }

            var viewModel = _mappingService.Map<StandardProviderSearchResponse, ProviderStandardSearchResultViewModel>(response, opt => opt
                .AfterMap((src, dest) => dest.AbsolutePath = Request?.Url?.AbsolutePath));

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
                        new { HasError = true, frameworkId = criteria?.ApprenticeshipId, postCode = criteria.PostCode });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.PostCodeInvalidFormat:
                    url = Url.Action(
                        "SearchForFrameworkProviders",
                        "Apprenticeship",
                        new { WrongPostcode = true, frameworkId = criteria?.ApprenticeshipId, postCode = criteria?.PostCode });
                    return new RedirectResult(url);

                case FrameworkProviderSearchResponse.ResponseCodes.PageNumberOutOfUpperBound:
                    url = Url.Action(
                        "FrameworkResults",
                        "Provider",
                        GenerateProviderResultsRouteValues(criteria, response.CurrentPage));

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
                    _logger.Warn($"404 - Cannot find provider: ({criteria.Ukprn}) for apprenticeship product: ({criteria.FrameworkId ?? criteria.StandardCode}) with location: ({criteria.LocationId})");
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                case DetailProviderResponse.ResponseCodes.InvalidInput:
                    _logger.Warn($"400 - Bad Request: {criteria.Ukprn}");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = _mappingService.Map<DetailProviderResponse, ApprenticeshipDetailsViewModel>(response, opt => opt
                                .AfterMap((src, dest) => dest.SurveyUrl = _settings.SurveyUrl.ToString()));

            return View(viewModel);
        }

        private static RouteValueDictionary GenerateProviderResultsRouteValues(ProviderSearchQuery criteria, int currentPage)
        {
            return new RouteValueDictionary()
                .AddValue("page", currentPage)
                .AddValue("postcode", criteria?.PostCode ?? string.Empty)
                .AddValue("apprenticeshipId", criteria?.ApprenticeshipId)
                .AddValue("showall", criteria?.ShowAll)
                .AddList("deliverymodes", criteria?.DeliveryModes);
        }
    }
}