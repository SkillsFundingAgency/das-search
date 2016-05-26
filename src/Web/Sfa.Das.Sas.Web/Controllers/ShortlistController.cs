using System;
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

        public ActionResult AddStandard(int id, string responseUrlParameters)
        {
            _logger.Debug($"Adding standard {id} to shortlist cookie");
            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = id
            };
            _listCollection.AddItem(Constants.StandardsShortListCookieName, shorlistedApprenticeship);

            return GetReturnRedirectFromStandardShortlistAction(id, responseUrlParameters);
        }

        public ActionResult AddStandardProvider(int standardId, string providerId, int locationId, string responseUrlParameters)
        {
            _logger.Debug($"Adding sprovider {providerId} with location {locationId} to apprenticeship {standardId} shortlist cookie");
            var shorlistedApprenticeship = new ShortlistedApprenticeship
            {
                ApprenticeshipId = standardId,
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

            return GetReturnRedirectFromStandardProviderShortlistAction(standardId, providerId, locationId, responseUrlParameters);
        }

        public ActionResult RemoveStandard(int id, string responseUrlParameters)
        {
            _logger.Debug($"Removing standard {id} from shortlist cookie");
            _listCollection.RemoveApprenticeship(Constants.StandardsShortListCookieName, id);

            return GetReturnRedirectFromStandardShortlistAction(id, responseUrlParameters);
        }

        public ActionResult RemoveStandardProvider(int standardId, string providerId, int locationId, string responseUrlParameters)
        {
            var provider = new ShortlistedProvider
            {
                ProviderId = providerId,
                LocationId = locationId
            };

            _logger.Debug($"Removing provider {provider.ProviderId} with location {provider.LocationId} from apprenticeship {standardId} shortlist cookie");
            _listCollection.RemoveProvider(Constants.StandardsShortListCookieName, standardId, provider);


            return GetReturnRedirectFromStandardProviderShortlistAction(standardId, providerId, locationId, responseUrlParameters);
        }

        private static string AppendUrlParametersToUrl(string url, string responseParameters)
        {
            if (string.IsNullOrEmpty(responseParameters))
            {
                return url;
            }

            var startOfParametersIndex = url.IndexOf("?", StringComparison.CurrentCultureIgnoreCase);

            if (startOfParametersIndex > 0)
            {
                url = url.Remove(startOfParametersIndex);
            }

            url += "?" + responseParameters;

            return url;
        }

        // This method is used to try to redirect back from the page that requested the updating of the
        // standards shortlist. If a URL cannot be found in the request then the default is to go back to
        // the standard details page itself.
        private ActionResult GetReturnRedirectFromStandardShortlistAction(int id, string responseParameters)
        {
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Standard", "Apprenticeship", new { id });
            }

            var url = Request.UrlReferrer.OriginalString;

            url = AppendUrlParametersToUrl(url, responseParameters);

            return Redirect(url);
        }

        // This method is used to try to redirect back from the page that requested the updating of the 
        // provider shortlist. If a URL cannot be found in the request then the default is to go back to 
        // the apprenticeship search page
        private ActionResult GetReturnRedirectFromStandardProviderShortlistAction(int standardId, string providerId, int locationId, string responseParameters)
        {
            if (Request.UrlReferrer == null)
            {
                var providerSearchCriteria = new ProviderLocationSearchCriteria()
                {
                    StandardCode = standardId.ToString(),
                    ProviderId = providerId,
                    LocationId = locationId.ToString()
                };

                return RedirectToAction("Detail", "Provider", new { providerSearchCriteria });
            }

            var url = Request.UrlReferrer.OriginalString;

            url = AppendUrlParametersToUrl(url, responseParameters);

            return Redirect(url);
        }
    }
}