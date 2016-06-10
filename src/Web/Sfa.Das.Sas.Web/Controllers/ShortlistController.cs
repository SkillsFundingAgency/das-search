using System.Collections.Generic;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Models;

namespace Sfa.Das.Sas.Web.Controllers
{
    public class ShortlistController : Controller
    {
        private readonly ILog _logger;
        private readonly IListCollection<int> _listCollection;

        public ShortlistController(ILog logger, IListCollection<int> listCollection)
        {
            _logger = logger;
            _listCollection = listCollection;
        }

        public ActionResult AddStandard(int id)
        {
            _logger.Debug($"Adding standard {id} to shortlist cookie");

            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = id
            };
            _listCollection.AddItem(Constants.StandardsShortListCookieName, shorlistedApprenticeship);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult AddStandardProvider(int apprenticeshipId, string providerId, int locationId)
        {
            _logger.Debug($"Adding sprovider {providerId} with location {locationId} to apprenticeship {apprenticeshipId} shortlist cookie");

            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = apprenticeshipId,
                ProvidersIdAndLocation = new List<ShortlistedProvider>
                {
                    new ShortlistedProvider
                    {
                        ProviderId = providerId,
                        LocationId = locationId
                    }
                }
            };

            _listCollection.AddItem(Constants.StandardsShortListCookieName, shorlistedApprenticeship);

            return GetReturnRedirectFromStandardProviderShortlistAction(apprenticeshipId, providerId, locationId);
        }

        public ActionResult RemoveStandard(int id)
        {
            _logger.Debug($"Removing standard {id} from shortlist cookie");

            _listCollection.RemoveApprenticeship(Constants.StandardsShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult RemoveStandardProvider(int apprenticeshipId, string providerId, int locationId)
        {
            var provider = new ShortlistedProvider
            {
                ProviderId = providerId,
                LocationId = locationId
            };

            _logger.Debug($"Removing provider {provider.ProviderId} with location {provider.LocationId} from apprenticeship {apprenticeshipId} shortlist cookie");
            _listCollection.RemoveProvider(Constants.StandardsShortListCookieName, apprenticeshipId, provider);

            return GetReturnRedirectFromStandardProviderShortlistAction(apprenticeshipId, providerId, locationId);
        }

        public ActionResult AddFramework(int id)
        {
            _logger.Debug($"Adding framework {id} to shortlist cookie");

            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = id
            };
            _listCollection.AddItem(Constants.FrameworksShortListCookieName, shorlistedApprenticeship);

            return GetReturnRedirectFromFrameworkShortlistAction(id);
        }

        public ActionResult RemoveFramework(int id)
        {
            _logger.Debug($"Removing framework {id} from shortlist cookie");

            _listCollection.RemoveApprenticeship(Constants.FrameworksShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        // This method is used to try to redirect back from the page that requested the updating of the
        // standards shortlist. If a URL cannot be found in the request then the default is to go back to
        // the standard details page itself.
        private ActionResult GetReturnRedirectFromStandardShortlistAction(int id)
        {
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Standard", "Apprenticeship", new { id });
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }

        // This method is used to try to redirect back from the page that requested the updating of the 
        // provider shortlist. If a URL cannot be found in the request then the default is to go back to 
        // the apprenticeship search page
        private ActionResult GetReturnRedirectFromStandardProviderShortlistAction(int standardId, string providerId, int locationId)
        {
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.OriginalString);
            }

            var providerSearchCriteria = new ProviderLocationSearchCriteria
            {
                StandardCode = standardId.ToString(),
                ProviderId = providerId,
                LocationId = locationId.ToString()
            };

            return RedirectToAction("Detail", "Provider", new { providerSearchCriteria });
        }

        private ActionResult GetReturnRedirectFromFrameworkShortlistAction(int id)
        {
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Framework", "Apprenticeship", new { id });
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }
    }
}