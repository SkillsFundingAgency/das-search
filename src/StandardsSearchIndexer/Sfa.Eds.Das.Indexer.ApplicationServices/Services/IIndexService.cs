using System;
using System.Collections.Generic;
using Nest;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    public interface IIndexMaintenanceService
    {
        string GetIndexNameAndDateExtension(DateTime dateTime, string indexName, string dateFormat = "yyyy-MM-dd-HH");
        List<string> GetOldIndices(string aliasName, DateTime scheduledRefreshDateTime, Dictionary<string, Stats> indices);
    }
}