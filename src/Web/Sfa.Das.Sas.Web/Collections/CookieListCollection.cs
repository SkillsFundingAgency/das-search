using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Web.Factories;

namespace Sfa.Das.Sas.Web.Collections
{
    public class CookieListCollection : IListCollection<int>
    {
        private readonly IConfigurationSettings _settings;
        private readonly IHttpCookieFactory _cookieFactory;

        public CookieListCollection(IConfigurationSettings settings, IHttpCookieFactory cookieFactory)
        {
            _settings = settings;
            _cookieFactory = cookieFactory;
        }

        public ICollection<int> GetAllItems(string listName)
        {
            var listCookie = GetListCookie(listName);
            return GetListItems(listCookie);
        }

        public void AddItem(string listName, int item)
        {
            var listCookie = GetListCookie(listName);

            var listItems = GetListItems(listCookie);

            if (!listItems.Any(x => x.Equals(item)))
            {
                listItems.Add(item);
            }

            listItems.Sort();

            var listString = CovertItemListToString(listItems);

            AddListToResponse(listName, listString);
        }

        public void RemoveItem(string listName, int item)
        {
            var listCookie = GetListCookie(listName);

            var listItems = GetListItems(listCookie);

            listItems.Remove(item);

            if (!listItems.Any())
            {
                RemoveList(listName);
                return;
            }

            listItems.Sort();

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

        private static List<int> GetListItems(HttpCookie cookie)
        {
            var listItems = new List<int>();

            if (string.IsNullOrEmpty(cookie.Value))
            {
                return listItems;
            }

            var itemStrings = cookie.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var stringValue in itemStrings)
            {
                int value;

                if (int.TryParse(stringValue, out value))
                {
                    listItems.Add(value);
                }
            }

            return listItems;
        }

        private static string CovertItemListToString(List<int> listItems)
        {
            if (!listItems.Any())
            {
                return string.Empty;
            }

            var listString = listItems.Select(x => x.ToString())
                .Aggregate((x1, x2) => x1 + "," + x2);

            return listString;
        }

        private void AddListToResponse(string listName, string listString)
        {
            var responseCookie = new HttpCookie(listName)
            {
                Value = listString,
                HttpOnly = true,
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