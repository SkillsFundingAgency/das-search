using System;

namespace Sfa.Eds.Das.Indexer.ApplicationServices
{
    internal static class IndexerHelper
    {
        public static string GetIndexNameAndDateExtension(DateTime dateTime, string indexName, string dateFormat = "yyyy-MM-dd-HH")
        {
            return $"{indexName}-{dateTime.ToUniversalTime().ToString(dateFormat)}".ToLower();
        }
    }
}
