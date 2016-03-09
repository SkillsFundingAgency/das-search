namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Nest;

    public class IndexMaintenanceService : IIndexMaintenanceService
    {
        public List<string> GetOldIndices(string aliasName, DateTime scheduledRefreshDateTime, Dictionary<string, Stats> indices)
        {
            var oneDayAgo2 = GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-1), aliasName, "yyyy-MM-dd");
            var twoDaysAgo2 = GetIndexNameAndDateExtension(scheduledRefreshDateTime.AddDays(-2), aliasName, "yyyy-MM-dd");

            return indices
                   .Where(index => index.Key.StartsWith(oneDayAgo2) || index.Key.StartsWith(twoDaysAgo2))
                   .Select(m => m.Key).ToList();

        }

        public string GetIndexNameAndDateExtension(DateTime dateTime, string indexName, string dateFormat = "yyyy-MM-dd-HH")
        {
            return $"{indexName}-{dateTime.ToUniversalTime().ToString(dateFormat)}".ToLower();
        }
    }
}