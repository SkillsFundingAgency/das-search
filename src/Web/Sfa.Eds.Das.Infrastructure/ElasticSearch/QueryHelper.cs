namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System.Text.RegularExpressions;

    internal static class QueryHelper
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
