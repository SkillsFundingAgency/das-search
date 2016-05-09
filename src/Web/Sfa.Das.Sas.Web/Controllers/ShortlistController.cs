using System.Web.Mvc;
using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Common;

namespace Sfa.Das.Sas.Web.Controllers
{
    public class ShortlistController : Controller
    {
        private readonly ILog _logger;
        private readonly IListCollection<int> _listCollection;
        private readonly IGetStandards _getStandards;

        public ShortlistController(
            IGetStandards getStandards,
            ILog logger,
            IListCollection<int> listCollection)
        {
            _getStandards = getStandards;
            _logger = logger;
            _listCollection = listCollection;
        }
       
        public ActionResult AddStandard(int id)
        {
            var standardResult = _getStandards.GetStandardById(id);

            // We only add standards that can currently be found
            if (standardResult == null)
            {
                return GetReturnRedirectFromStandardShortlistAction(id);
            }

            _listCollection.AddItem(Constants.StandardsShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult RemoveStandard(int id)
        {
            _listCollection.RemoveItem(Constants.StandardsShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        // This method is used to try to redirect back from the page that requested the updating of the 
        // standards shortlist. If a URL cannot be found in the request then the default is to go back to 
        // the standard details page itself. 
        private ActionResult GetReturnRedirectFromStandardShortlistAction(int id)
        {
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.OriginalString);
            }

            return RedirectToAction("Standard", "Apprenticeship", new { id = id });
        }
    }
}