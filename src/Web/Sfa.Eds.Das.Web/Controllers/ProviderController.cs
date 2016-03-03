using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;

    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public sealed class ProviderController : Controller
    {
        private readonly IProviderSearchService _providerSearchService;
        private readonly ILog _logger;
        private readonly IMappingService _mappingService;

        public ProviderController(IProviderSearchService providerSearchService, ILog logger, IMappingService mappingService)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
        }

        [HttpGet]
        public async Task<ActionResult> SearchResults(ProviderSearchCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria?.PostCode))
            {
                return RedirectToAction("Detail", "Standard", new { id = criteria.StandardId, HasError = true });
            }

            var searchResults = await _providerSearchService.SearchByPostCode(criteria.StandardId, criteria.PostCode);

            var viewModel = _mappingService.Map<ProviderSearchResults, ProviderSearchResultViewModel>(searchResults);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}