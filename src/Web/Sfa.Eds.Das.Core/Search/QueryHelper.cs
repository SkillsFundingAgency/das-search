namespace Sfa.Eds.Das.Core.Search
{
    using System.Text.RegularExpressions;

    class QueryHelper
    {
        protected internal static string FormatQuery(string query, bool toLower = true)
        {
            query = Regex.Replace(query, @"[+\-&|!(){}\[\]^""~?:\\/]", @" ").Trim();
            query = query == string.Empty ? "*" : query;
            return toLower ? query.ToLowerInvariant() : query;
        }
    }
}
