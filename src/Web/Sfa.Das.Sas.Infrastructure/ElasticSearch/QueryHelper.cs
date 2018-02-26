using System.Text.RegularExpressions;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    internal static class QueryHelper
    {
        internal static string FormatQuery(string query, bool toLower = true)
        {
            if (string.IsNullOrEmpty(query))
            {
                return "*";
            }

            return ReplaceUnacceptableCharacters(query, toLower);
        }

        internal static string FormatQueryReturningEmptyStringIfEmptyOrNull(string query, bool toLower = true)
        {
            if (string.IsNullOrEmpty(query))
            {
                return string.Empty;
            }

            return ReplaceUnacceptableCharacters(query, toLower);
        }

        private static string ReplaceUnacceptableCharacters(string query, bool toLower)
        {
            var queryformated = Regex.Replace(query, @"[+\-&|!(){}\[\]^""~?:\\/]", @" ");

            return toLower ? queryformated.ToLowerInvariant() : queryformated;
        }
    }
}
