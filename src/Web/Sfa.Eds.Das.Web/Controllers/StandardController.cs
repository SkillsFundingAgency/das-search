namespace Sfa.Eds.Das.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using log4net;

    using Sfa.Eds.Das.Core.Interfaces.Search;
    using Sfa.Eds.Das.Core.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public class StandardController : Controller
    {
        private readonly ISearchService searchService;

        private readonly ILog logger;

        private readonly IMappingService mappingService;

        public StandardController(ISearchService searchService, ILog logger, IMappingService mappingService)
        {
            this.searchService = searchService;
            this.logger = logger;
            this.mappingService = mappingService;
        }

        // GET: Standard
        public ActionResult Detail(string id)
        {
            var standardResult = this.searchService.GetStandardItem(id);

            if (standardResult == null)
            {
                var message = $"Cannot find standard: {id}";
                this.logger.Warn($"404 - {message}");
                return new HttpNotFoundResult(message);
            }

            var viewModel = this.mappingService.Map<SearchResultsItem, StandardViewModel>(standardResult);
            viewModel.SearchResultLink = GetSearchResultUrl(Request.UrlReferrer);
            return View(viewModel);
        }

        private LinkViewModel GetSearchResultUrl(Uri urlReferrer)
        {
            if (urlReferrer != null && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("?keywords"))
            {
                return new LinkViewModel { Title = "Results", Url = urlReferrer.OriginalString };
            }

            return new LinkViewModel { Title = "Back to search page", Url = Url.Action("Index", "Home") };
        }
    }
}