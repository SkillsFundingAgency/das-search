namespace Sfa.Das.Sas.Web.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Routing;

    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary AddValue(this RouteValueDictionary dictionary, string key, object value)
        {
            if (value == null)
            {
                return dictionary;
            }

            dictionary.Add(key, value);
            return dictionary;
        }

        public static RouteValueDictionary AddList(this RouteValueDictionary dictionary, string key, IEnumerable<string> values)
        {
            var enumerable = values?.ToArray();
            if (enumerable.IsNullOrEmpty())
            {
                return dictionary;
            }

            for (int i = 0; i < enumerable.Count(); i++)
            {
                dictionary.Add($"{key}[{i}]", enumerable.ToArray()[i]);
            }

            return dictionary;
        }
    }
}