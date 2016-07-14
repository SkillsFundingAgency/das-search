using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using MediatR;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Attribute;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    [NoCache]
    public sealed class ApprenticeshipController : Controller
    {
        private readonly ILog _logger;
        private readonly IApprenticeshipViewModelFactory _apprenticeshipViewModelFactory;
        private readonly IMediator _mediator;

        public ApprenticeshipController(
            ILog logger,
            IApprenticeshipViewModelFactory apprenticeshipViewModelFactory,
            IMediator mediator)
        {
            _logger = logger;
            _apprenticeshipViewModelFactory = apprenticeshipViewModelFactory;
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

            var viewModel = _apprenticeshipViewModelFactory.GetApprenticeshipSearchResultViewModel(response);

            if (response.StatusCode == ApprenticeshipSearchResponse.ResponseCodes.SearchPageLimitExceeded)
            {
                var rv = new RouteValueDictionary { { "keywords", query.Keywords }, { "page", response.LastPage } };
                var index = 0;

                foreach (var level in viewModel.AggregationLevel.Where(m => m.Checked))
                {
                    rv.Add("SelectedLevels[" + index + "]", level.Value);
                    index++;
                }

                var url = Url.Action("SearchResults", "Apprenticeship", rv);

                return new RedirectResult(url);
            }

            if (viewModel != null)
            {
                return View(viewModel);
            }

            _logger.Warn("ViewModel is null, SearchResults, ApprenticeshipController ");

            return View(new ApprenticeshipSearchResultViewModel());
        }

        // GET: Standard
        public ActionResult Standard(int id, string keywords)
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

            var viewModel = _apprenticeshipViewModelFactory.GetStandardViewModel(response.Standard);

            viewModel.SearchTerm = response.SearchTerms;
            viewModel.IsShortlisted = response.IsShortlisted;

            return View(viewModel);
        }

        public ActionResult Framework(int id, string keywords)
        {
            var response = _mediator.Send(new GetStandardQuery { Id = id, Keywords = keywords });

            if (response.StatusCode == GetStandardResponse.ResponseCodes.InvalidStandardId)
            {
                _logger.Info("404 - Attempt to get standard with an ID below zero");
                return HttpNotFound("Cannot find any standards with an ID below zero");
            }

            if (response.StatusCode == GetStandardResponse.ResponseCodes.StandardNotFound)
            {
                var message = $"Cannot find framework: {id}";
                _logger.Warn($"404 - {message}");

                return new HttpNotFoundResult(message);
            }

            var viewModel = _apprenticeshipViewModelFactory.GetStandardViewModel(response.Standard);

            viewModel.SearchTerm = response.SearchTerms;
            viewModel.IsShortlisted = response.IsShortlisted;

            return View(viewModel);
        }

        public ActionResult SearchForProviders(int? standardId, int? frameworkId, string postCode, string keywords, string hasError)
        {
            ProviderSearchViewModel viewModel;

            if (standardId != null)
            {
                viewModel = _apprenticeshipViewModelFactory.GetProviderSearchViewModelForStandard(standardId.Value, @Url);
            }
            else if (frameworkId != null)
            {
                viewModel = _apprenticeshipViewModelFactory.GetFrameworkProvidersViewModel(frameworkId.Value, @Url);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }

            viewModel.HasError = !string.IsNullOrEmpty(hasError) && bool.Parse(hasError);
            viewModel.PostCode = postCode;
            viewModel.SearchTerms = keywords;

            return View(viewModel);
        }
    }
}