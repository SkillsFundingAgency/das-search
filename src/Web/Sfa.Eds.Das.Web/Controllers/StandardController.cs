namespace Sfa.Eds.Das.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using log4net;

    using Sfa.Eds.Das.Web.Models.ViewModels;
    using Sfa.Eds.Das.Web.Services;

    public class StandardController : Controller
    {
        private readonly ISearchForStandards searchService;

        private readonly ILog logger;

        public StandardController(ISearchForStandards searchService, ILog logger)
        {
            this.searchService = searchService;
            this.logger = logger;
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

            // Part of the page not a standard - ViewModel with automapper?
            standardResult.SearchResultLink = GetSearchResultUrl(Request.UrlReferrer);

            return View(standardResult);
        }

        private LinkViewModel GetSearchResultUrl(Uri urlReferrer)
        {
            if (urlReferrer != null)
            {
                if (urlReferrer.OriginalString.ToLower().Contains("?keywords"))
                {
                    return new LinkViewModel() { Title = "Results", Url = urlReferrer.OriginalString };
                }
            }

            return new LinkViewModel() { Title = "Back to search page", Url = Url.Action("Index", "Home") };
        }
    }
}