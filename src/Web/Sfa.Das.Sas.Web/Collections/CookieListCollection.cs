using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Models;

namespace Sfa.Das.Sas.Web.Collections
{
    using Sfa.Das.Sas.ApplicationServices.Settings;

    public class CookieListCollection : IListCollection<int>
    {
        private readonly ICookieSettings _settings;
        private readonly IHttpCookieFactory _cookieFactory;

        public CookieListCollection(ICookieSettings settings, IHttpCookieFactory cookieFactory)
        {
            _settings = settings;
            _cookieFactory = cookieFactory;
        }

        public ICollection<ShortlistedApprenticeship> GetAllItems(string listName)
        {
            var listCookie = GetListCookie(listName);
            return GetListItems(listCookie);
        }

        public void AddItem(string listName, ShortlistedApprenticeship item)
        {
            var listCookie = GetListCookie(listName);

            var listItems = GetListItems(listCookie);

            if (item.ProvidersId == null)
            {
                item.ProvidersId = new List<int>();
            }

            if (!listItems.Any(x => x.ApprenticeshipId.Equals(item.ApprenticeshipId)))
            {
                listItems.Add(item);
            }
            else
            {
                foreach (var shortlistedApprenticeship in listItems)
                {
                    if (shortlistedApprenticeship.ApprenticeshipId.Equals(item.ApprenticeshipId))
                    {
                        foreach (var providerId in item.ProvidersId.Where(providerId => !shortlistedApprenticeship.ProvidersId.Any(x => x.Equals(providerId))))
                        {
                            shortlistedApprenticeship.ProvidersId.Add(providerId);
                        }
                    }
                }
            }

            var listString = CovertItemListToString(listItems);

            AddListToResponse(listName, listString);
        }

        public void RemoveApprenticeship(string listName, int item)
        {
            var listCookie = GetListCookie(listName);

            var listItems = GetListItems(listCookie);

            foreach (var shortlistedApprenticeship in listItems.Where(shortlistedApprenticeship => shortlistedApprenticeship.ApprenticeshipId.Equals(item)))
            {
                listItems.Remove(shortlistedApprenticeship);
                break;
            }

            if (!listItems.Any())
            {
                RemoveList(listName);
                return;
            }

            var listString = CovertItemListToString(listItems);
            AddListToResponse(listName, listString);
        }

        public void RemoveProvider(string listName, int item)
        {
            var listCookie = GetListCookie(listName);

            var listItems = GetListItems(listCookie);

            foreach (var shortlistedApprenticeship in listItems)
            {
                foreach (var provider in shortlistedApprenticeship.ProvidersId.Where(provider => provider.Equals(item)))
                {
                    shortlistedApprenticeship.ProvidersId.Remove(provider);
                }
            }

            if (!listItems.Any())
            {
                RemoveList(listName);
                return;
            }

            var listString = CovertItemListToString(listItems);
            AddListToResponse(listName, listString);
        }

        public void RemoveList(string listName)
        {
            var requestCookies = _cookieFactory.GetRequestCookies();

            if (requestCookies[listName] == null)
            {
                return;
            }

            var cookie = new HttpCookie(listName)
            {
                Expires = DateTime.Now.AddDays(-1),
                Domain = _settings.CookieDomain
            };

            var responseCookies = _cookieFactory.GetResponseCookies();

            responseCookies.Add(cookie);
        }

        private static List<ShortlistedApprenticeship> GetListItems(HttpCookie cookie)
        {
            var listItems = new List<ShortlistedApprenticeship>();

            if (string.IsNullOrEmpty(cookie.Value))
            {
                return listItems;
            }

            var shortlistedApprenticeships = SplitCoockie(cookie);

            foreach (var shortlistedApprenticeship in shortlistedApprenticeships)
            {
                var splittedApprenticeships = SplitShortlistedApprenticeship(shortlistedApprenticeship);

                var prov = splittedApprenticeships.Count() > 1 ? SplitProviderIds(splittedApprenticeships.ElementAt(1)) : new List<string>();

                var providers = new List<int>();
                foreach (var p in prov)
                {
                    int value;
                    if (int.TryParse(p, out value))
                    {
                        providers.Add(value);
                    }
                }

                int apprenticeshipId;
                int.TryParse(splittedApprenticeships.ElementAt(0), out apprenticeshipId);

                listItems.Add(new ShortlistedApprenticeship
                {
                    ApprenticeshipId = apprenticeshipId,
                    ProvidersId = providers
                });
            }

            return listItems;
        }

        private static IEnumerable<string> SplitCoockie(HttpCookie cookie)
        {
            return cookie.Value.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static IEnumerable<string> SplitShortlistedApprenticeship(string shortlistedApprenticeship)
        {
            return shortlistedApprenticeship.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static IEnumerable<string> SplitProviderIds(string s)
        {
            return s.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string CovertItemListToString(List<ShortlistedApprenticeship> listItems)
        {
            if (!listItems.Any())
            {
                return string.Empty;
            }

            var listShortlistedProviders = new List<string>();
            foreach (var shortlistedApprenticeship in listItems)
            {
                var text = new StringBuilder();
                var count = 0;
                foreach (var providerId in shortlistedApprenticeship.ProvidersId)
                {
                    text.Append(count == 0 ? providerId.ToString() : string.Concat("|", providerId.ToString()));
                    count++;
                }

                listShortlistedProviders.Add(string.Format("{0}={1}", shortlistedApprenticeship.ApprenticeshipId, text));
            }

            var listString = listShortlistedProviders.Select(x => x)
                .Aggregate((x1, x2) => x1 + "&" + x2);

            return listString;
        }

        private void AddListToResponse(string listName, string listString)
        {
            var responseCookie = new HttpCookie(listName)
            {
                Value = listString,
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(1),
                Secure = _settings.UseSecureCookies,
                Domain = _settings.CookieDomain
            };

            var responseCookies = _cookieFactory.GetResponseCookies();

            responseCookies.Add(responseCookie);
        }

        private HttpCookie GetListCookie(string listName)
        {
            var requestCookies = _cookieFactory.GetRequestCookies();

            var listCookie = requestCookies[listName] ?? new HttpCookie(listName);
            return listCookie;
        }
    }
}