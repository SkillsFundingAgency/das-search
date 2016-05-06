using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfa.Das.Sas.Core.Collections;

namespace Sfa.Das.Sas.Web.Collections
{
    public class CookieListCollection : IListCollection<int>
    {
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
            if (HttpContext.Current.Request.Cookies[listName] == null)
            {
                return;
            }

            var cookie = new HttpCookie(listName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
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

        private static void AddListToResponse(string listName, string listString)
        {
            var responseCookie = new HttpCookie(listName)
            {
                Value = listString
            };

            HttpContext.Current.Response.Cookies.Add(responseCookie);
        }

        private static HttpCookie GetListCookie(string listName)
        {
            var cookies = HttpContext.Current.Request.Cookies;

            var listCookie = cookies[listName] ?? new HttpCookie(listName);
            return listCookie;
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
    }
}