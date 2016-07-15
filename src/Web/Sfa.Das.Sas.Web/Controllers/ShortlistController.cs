using System.Collections.Generic;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Logging;
using Sfa.Das.Sas.Web.Models;

namespace Sfa.Das.Sas.Web.Controllers
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class ShortlistController : Controller
    {
        private readonly ILog _logger;
        private readonly IShortlistCollection<int> _shortlistCollection;

        public ShortlistController(ILog logger, IShortlistCollection<int> shortlistCollection)
        {
            _logger = logger;
            _shortlistCollection = shortlistCollection;
        }

        public ActionResult AddStandard(int id)
        {
            _logger.Debug($"Adding standard to shortlist", new ShortlistLogEntry { StandardId = id });

            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = id
            };

            _shortlistCollection.AddItem(Constants.StandardsShortListCookieName, shorlistedApprenticeship);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult AddStandardProvider(int apprenticeshipId, int ukprn, int locationId)
        {
            var logEntry = new ShortlistLogEntry
            {
                StandardId = apprenticeshipId,
                Ukprn = ukprn,
                LocationId = locationId
            };

            _logger.Debug("Adding standard provider to shortlist", logEntry);

            var shortlistedApprenticeshipProvider = CreateShortlistedApprenticeship(apprenticeshipId, ukprn, locationId);

            _shortlistCollection.AddItem(Constants.StandardsShortListCookieName, shortlistedApprenticeshipProvider);

            var providerSearchCriteria = new ApprenticeshipShortlistCriteria
            {
                StandardCode = apprenticeshipId.ToString(),
                Ukprn = ukprn,
                LocationId = locationId.ToString()
            };

            return GetReturnRedirectFromProviderShortlistAction(providerSearchCriteria);
        }

        public ActionResult RemoveStandard(int id)
        {
            _logger.Debug("Removing standard from shortlist", new ShortlistLogEntry { StandardId = id });

            _shortlistCollection.RemoveApprenticeship(Constants.StandardsShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult RemoveStandardProvider(int apprenticeshipId, int ukprn, int locationId)
        {
            var logEntry = new ShortlistLogEntry
            {
                StandardId = apprenticeshipId,
                LocationId = locationId,
                Ukprn = ukprn
            };

            _logger.Debug($"Removing standard provider from shortlist", logEntry);

            var provider = new ShortlistedProvider
            {
                Ukprn = ukprn,
                LocationId = locationId
            };

            _shortlistCollection.RemoveProvider(Constants.StandardsShortListCookieName, apprenticeshipId, provider);

            var providerSearchCriteria = new ApprenticeshipShortlistCriteria
            {
                StandardCode = apprenticeshipId.ToString(),
                Ukprn = ukprn,
                LocationId = locationId.ToString()
            };

            return GetReturnRedirectFromProviderShortlistAction(providerSearchCriteria);
        }

        public ActionResult AddFramework(int id)
        {
            _logger.Debug($"Adding framework to shortlist", new ShortlistLogEntry { FrameworkId = id });

            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = id
            };

            _shortlistCollection.AddItem(Constants.FrameworksShortListCookieName, shorlistedApprenticeship);

            return GetReturnRedirectFromFrameworkShortlistAction(id);
        }

        public ActionResult AddFrameworkProvider(int apprenticeshipId, int ukprn, int locationId)
        {
            var logEntry = new ShortlistLogEntry
            {
                FrameworkId = apprenticeshipId,
                Ukprn = ukprn,
                LocationId = locationId
            };

            _logger.Debug("Adding framework provider to shortlist", logEntry);

            var shortListedApprenticeshipProvider = CreateShortlistedApprenticeship(apprenticeshipId, ukprn, locationId);

            _shortlistCollection.AddItem(Constants.FrameworksShortListCookieName, shortListedApprenticeshipProvider);

            var providerSearchCriteria = new ApprenticeshipShortlistCriteria
            {
                FrameworkId = apprenticeshipId.ToString(),
                Ukprn = ukprn,
                LocationId = locationId.ToString()
            };

            return GetReturnRedirectFromProviderShortlistAction(providerSearchCriteria);
        }

        public ActionResult RemoveFramework(int id)
        {
            _logger.Debug($"Removing framework from shortlist", new ShortlistLogEntry { FrameworkId = id });

            _shortlistCollection.RemoveApprenticeship(Constants.FrameworksShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id);
        }

        public ActionResult RemoveFrameworkProvider(int apprenticeshipId, int ukprn, int locationId)
        {
            var logEntry = new ShortlistLogEntry
            {
                FrameworkId = apprenticeshipId,
                LocationId = locationId,
                Ukprn = ukprn
            };

            _logger.Debug($"Removing framework provider from shortlist", logEntry);

            var provider = new ShortlistedProvider
            {
                Ukprn = ukprn,
                LocationId = locationId
            };

            _shortlistCollection.RemoveProvider(Constants.FrameworksShortListCookieName, apprenticeshipId, provider);

            var providerSearchCriteria = new ApprenticeshipShortlistCriteria
            {
                FrameworkId = apprenticeshipId.ToString(),
                Ukprn = ukprn,
                LocationId = locationId.ToString()
            };

            return GetReturnRedirectFromProviderShortlistAction(providerSearchCriteria);
        }

        private static ShortlistedApprenticeship CreateShortlistedApprenticeship(int apprenticeshipId, int ukprn, int locationId)
        {
            return new ShortlistedApprenticeship
            {
                ApprenticeshipId = apprenticeshipId,
                ProvidersUkrpnAndLocation = new List<ShortlistedProvider>
                {
                    new ShortlistedProvider
                    {
                        Ukprn = ukprn,
                        LocationId = locationId
                    }
                }
            };
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
        private ActionResult GetReturnRedirectFromFrameworkShortlistAction(int id)
        {
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Framework", "Apprenticeship", new { id });
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }

        private ActionResult GetReturnRedirectFromProviderShortlistAction(ApprenticeshipShortlistCriteria criteria)
        {
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Detail", "Provider", new { criteria });
            }

            return Redirect(Request.UrlReferrer.OriginalString);
        }
    }
}