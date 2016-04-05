namespace Sfa.Eds.Das.Indexer.ApplicationServices
{
    using System;
    using System.Globalization;

    internal static class IndexerHelper
    {
        public static string GetIndexNameAndDateExtension(DateTime dateTime, string indexName, string dateFormat = "yyyy-MM-dd-HH-mm")
        {
            return $"{indexName}-{dateTime.ToString(dateFormat)}".ToLower(CultureInfo.InvariantCulture);
        }
    }
}