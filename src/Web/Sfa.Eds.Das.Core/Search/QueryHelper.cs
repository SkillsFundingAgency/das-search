namespace Sfa.Eds.Das.Core.Search
{
    using System.Text.RegularExpressions;

    static class QueryHelper
    {
        internal static string FormatQuery(string query, bool toLower = true)
        {
            if (string.IsNullOrEmpty(query))
            {
                return "*";
            }
            var queryformated = Regex.Replace(query, @"[+\-&|!(){}\[\]^""~?:\\/]", @" ");            
            return toLower ? queryformated.ToLowerInvariant() : queryformated;
        }
    }
}
