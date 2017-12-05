using System.Net;

namespace Sfa.Das.Sas.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using ApplicationServices.Queries;
    using ApplicationServices.Responses;
    using Attribute;
    using MediatR;
    using Services;
    using SFA.DAS.NLog.Logger;
    using ViewModels;

    [NoCache]
    public sealed class ApprenticeshipController : Controller
    {
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;
        private readonly IMediator _mediator;

        public ApprenticeshipController(
            ILog logger,
            IMappingService mappingService,
            IMediator mediator)
        {
            _logger = logger;
            _mappingService = mappingService;
            _mediator = mediator;
        }

        public ActionResult Search()
        {
            return View();
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
            var response = _mediator.Send(new GetStandardQuery {Id = id, Keywords = keywords});

            switch (response.StatusCode)
            {
                case GetStandardResponse.ResponseCodes.InvalidStandardId:
                {
                    _logger.Info("404 - Attempt to get standard with an ID below zero");
                    return HttpNotFound("Cannot find any standards with an ID below zero");
                }

                case GetStandardResponse.ResponseCodes.StandardNotFound:
                {
                    var message = $"Cannot find standard: {id}";
                    _logger.Warn($"404 - {message}");

                    return new HttpNotFoundResult(message);
                }

                case GetStandardResponse.ResponseCodes.AssessmentOrgsEntityNotFound:
                {
                    var message = $"Cannot find assessment organisations for standard: {id}";
                    _logger.Warn($"404 - {message}");
                    break;
                }

                case GetStandardResponse.ResponseCodes.HttpRequestException:
                {
                    var message = $"Request error when requesting assessment orgs for standard: {id}";
                    _logger.Warn($"400 - {message}");

                    return new HttpNotFoundResult(message);
                }
            }

            var viewModel = _mappingService.Map<GetStandardResponse, StandardViewModel>(response);

            return View(viewModel);
        }

        public ActionResult Framework(string id, string keywords)
        {
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
                    var viewModel = _mappingService.Map<GetFrameworkResponse, FrameworkViewModel>(response);

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
            viewModel.ErrorMessage = CreateErrorMessage(statusCode);
            _logger.Debug($"statusCode: {statusCode}");
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
            viewModel.ErrorMessage = CreateErrorMessage(statusCode);
            _logger.Debug($"statusCode: {statusCode}");
            viewModel.IsLevyPayingEmployer = isLevyPayingEmployer;

            return View("SearchForProviders", viewModel);
        }

        private string CreateErrorMessage(ProviderSearchResponseCodes? statusCode)
        {
            var postCodeNotInEngland = "The postcode entered is not in England. Information about apprenticeships in";
            switch (statusCode)
            {
                case ProviderSearchResponseCodes.LocationServiceUnavailable:
                    return "Sorry, postcode search not working, please try again later";
                case ProviderSearchResponseCodes.PostCodeTerminated:
                    return "Sorry, this postcode is no longer valid";
                case ProviderSearchResponseCodes.PostCodeInvalidFormat:
                    return "You must enter a full and valid postcode";
                case ProviderSearchResponseCodes.WalesPostcode:
                    return $"{postCodeNotInEngland} <a href=\"https://businesswales.gov.wales/skillsgateway/apprenticeships\">Wales</a>";
                case ProviderSearchResponseCodes.NorthernIrelandPostcode:
                    return $"{postCodeNotInEngland} <a href=\"https://www.nibusinessinfo.co.uk/content/apprenticeships-employers\">Northern Ireland</a>";
                case ProviderSearchResponseCodes.ScotlandPostcode:
                    return $"{postCodeNotInEngland} <a href=\"https://www.apprenticeships.scot/\">Scotland</a>";
            }

            return string.Empty;
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
    }
}