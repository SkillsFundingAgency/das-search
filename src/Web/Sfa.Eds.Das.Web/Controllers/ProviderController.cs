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
    using log4net;
    using System;
    public sealed class ProviderController : Controller
    {
        private static readonly log4net.ILog Log = LogManager.GetLogger("MainLogger");
        private readonly IProviderSearchService _providerSearchService;
        private readonly Core.Logging.ILog _logger;
        private readonly IMappingService _mappingService;
        
        public ProviderController(IProviderSearchService providerSearchService, Core.Logging.ILog logger, IMappingService mappingService)
        {
            _providerSearchService = providerSearchService;
            _logger = logger;
            _mappingService = mappingService;
        }

        [HttpGet]
        public async Task<ActionResult> SearchResults(ProviderSearchCriteria criteria)
        {
            try
            {
                if (string.IsNullOrEmpty(criteria?.PostCode))
                {
                    return RedirectToAction("Detail", "Standard", new { id = criteria.StandardId, HasError = true });
                }

                var searchResults = await _providerSearchService.SearchByPostCode(criteria.StandardId, criteria.PostCode);

                var viewModel = _mappingService.Map<ProviderSearchResults, ProviderSearchResultViewModel>(searchResults);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error("PostCode lookup failed", ex);
                throw;
            }
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}