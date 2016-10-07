using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Attribute;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
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
            var response = _mediator.Send(new GetStandardQuery { Id = id, Keywords = keywords });

            if (response.StatusCode == GetStandardResponse.ResponseCodes.InvalidStandardId)
            {
                _logger.Info("404 - Attempt to get standard with an ID below zero");
                return HttpNotFound("Cannot find any standards with an ID below zero");
            }

            if (response.StatusCode == GetStandardResponse.ResponseCodes.StandardNotFound)
            {
                var message = $"Cannot find standard: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var viewModel = _mappingService.Map<GetStandardResponse, StandardViewModel>(response);

            return View(viewModel);
        }

        public ActionResult Framework(string id, string keywords)
        {
            var response = _mediator.Send(new GetFrameworkQuery { Id = id, Keywords = keywords });

            switch (response.StatusCode)
            {
                case GetFrameworkResponse.ResponseCodes.InvalidFrameworkId:
                    _logger.Info("404 - Attempt to get standard with an ID below zero");

                    return HttpNotFound("Cannot find any standards with an ID below zero");

                case GetFrameworkResponse.ResponseCodes.FrameworkNotFound:
                    var message = $"Cannot find framework: {id}";

                    _logger.Warn($"404 - {message}");

                    return new HttpNotFoundResult(message);

                case GetFrameworkResponse.ResponseCodes.Success:
                    var viewModel = _mappingService.Map<GetFrameworkResponse, FrameworkViewModel>(response);

                    return View(viewModel);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ActionResult SearchForStandardProviders(string standardId, string wrongPostcode, string postcode, string keywords, string hasError)
        {
            var query = new GetStandardProvidersQuery
            {
                StandardId = standardId,
                Postcode = postcode,
                Keywords = keywords,
                HasErrors = hasError
            };

            var response = _mediator.Send(query);

            if (response.StatusCode.Equals(GetStandardProvidersResponse.ResponseCodes.NoStandardFound))
            {
                return new HttpNotFoundResult();
            }

            var viewModel = _mappingService.Map<GetStandardProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.PostUrl = Url?.Action("StandardResults", "Provider");
            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.WrongPostcode = !string.IsNullOrEmpty(wrongPostcode) && bool.Parse(wrongPostcode);

            return View("SearchForProviders", viewModel);
        }

        public ActionResult SearchForFrameworkProviders(string frameworkId, string wrongPostcode, string postcode, string keywords, string hasError)
        {
            var query = new GetFrameworkProvidersQuery
            {
                FrameworkId = frameworkId, Postcode = postcode, Keywords = keywords, HasErrors = hasError
            };

            var response = _mediator.Send(query);

            if (response.StatusCode.Equals(GetFrameworkProvidersResponse.ResponseCodes.NoFrameworkFound))
            {
                return new HttpNotFoundResult();
            }

            var viewModel = _mappingService.Map<GetFrameworkProvidersResponse, ProviderSearchViewModel>(response);

            viewModel.PostUrl = Url?.Action("FrameworkResults", "Provider");
            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.WrongPostcode = !string.IsNullOrEmpty(wrongPostcode) && bool.Parse(wrongPostcode);

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
    }
}