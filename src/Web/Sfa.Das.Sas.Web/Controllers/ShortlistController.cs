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
            _logger.Debug($"Adding standard {id} to shortlist cookie");
            _listCollection.AddItem(Constants.StandardsShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult RemoveStandard(int id)
        {
            _logger.Debug($"Removing standard {id} from shortlist cookie");
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