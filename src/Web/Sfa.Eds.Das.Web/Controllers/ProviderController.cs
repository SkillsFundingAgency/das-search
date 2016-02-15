namespace Sfa.Eds.Das.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public sealed class ProviderController : Controller
    {
        private readonly ISearchProvider _providerSearchService;

        private readonly ILog _logger;
        private readonly IMappingService _mappingService;

        public ProviderController(ISearchProvider providerSearchService, ILog logger, IMappingService mappingService)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
        }

        [HttpGet]
        public ActionResult SearchResults(ProviderSearchCriteria criteria)
        {
            var searchResults = _providerSearchService.SearchByLatLon(criteria.StandardId, criteria.Skip, criteria.Take, criteria.PostCode);

            var viewModel = _mappingService.Map<ProviderSearchResults, ProviderSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        // GET: Standard
        public ActionResult Detail(string id)
        {
            return View();
        }

        private LinkViewModel GetSearchResultUrl(Uri urlReferrer)
        {
            if (urlReferrer != null && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("?keywords"))
            {
                return new LinkViewModel { Title = "Results", Url = urlReferrer.OriginalString };
            }

            return new LinkViewModel { Title = "Back to search page", Url = Url.Action("Search", "Standard") };
        }
    }
}