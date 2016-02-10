namespace Sfa.Eds.Das.Core.Search
{
    using System.Text.RegularExpressions;

    static class QueryHelper
    {
        internal static string FormatQuery(string query, bool toLower = true)
        {
            var q = Regex.Replace(query, @"[+\-&|!(){}\[\]^""~?:\\/]", @" ").Trim();
            var queryformated = q == string.Empty ? "*" : query;
            return toLower ? queryformated.ToLowerInvariant() : queryformated;
        }
    }
}
