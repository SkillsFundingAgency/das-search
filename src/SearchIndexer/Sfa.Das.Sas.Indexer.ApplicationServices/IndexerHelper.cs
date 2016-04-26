using System;
using System.Globalization;

namespace Sfa.Das.Sas.Indexer.ApplicationServices
{
    internal static class IndexerHelper
    {
        public static string GetIndexNameAndDateExtension(DateTime dateTime, string indexName, string dateFormat = "yyyy-MM-dd-HH-mm")
        {
            return $"{indexName}-{dateTime.ToString(dateFormat)}".ToLower(CultureInfo.InvariantCulture);
        }
    }
}