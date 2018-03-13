using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class DeduplicationService : IDeduplicationService
    {
        public IEnumerable<T> DedupeAtYourLocationOnlyDocuments<T>(IEnumerable<T> documents)
            where T : IApprenticeshipProviderSearchResultsItem
        {
            var dedupedDocuments = new List<T>();
            var ukprnsToIgnore = new List<int>();

            foreach (var document in documents)
            {
                if (document?.DeliveryModes != null && document.DeliveryModes.Count == 1 && document.DeliveryModes[0] == "100PercentEmployer")
                {
                    if (ukprnsToIgnore.Count(x => x == document.Ukprn) == 0)
                    {
                        dedupedDocuments.Add(document);
                        ukprnsToIgnore.Add(document.Ukprn);
                    }
                }
                else
                {
                    dedupedDocuments.Add(document);
                }
            }

            return dedupedDocuments;
        }

        public Dictionary<string, long?> RetrieveTrainingOptionsAggregationElements<T>(IEnumerable<T> documents) 
            where T : IApprenticeshipProviderSearchResultsItem
        {
            //{[100percentemployer, 380]}
            //{[dayrelease, 88]}
            //{[blockrelease, 77]}
            var res = new Dictionary<string, long?>();

            var documentList = documents.ToList();
            long? count100PercentEmployer = documentList.Count(x => x.DeliveryModes.Contains("100PercentEmployer"));
            long? countDayRelease = documentList.Count(x => x.DeliveryModes.Contains("DayRelease"));
            long? countBlackRelease = documentList.Count(x => x.DeliveryModes.Contains("BlockRelease"));

            res.Add("100percentemployer", count100PercentEmployer);
            res.Add("dayrelease", countDayRelease);
            res.Add("blockrelease", countBlackRelease);

            return res;
        }

        public Dictionary<string, long?> RetrieveNationalProvidersAggregationElements<T>(IEnumerable<T> documents) 
            where T : IApprenticeshipProviderSearchResultsItem
        {
//            {[1, 377]}
//            {[0, 12]}
            var isNational = "1";
            var isNotNational = "0";
            var res = new Dictionary<string, long?>();
            var documentList = documents.ToList();

            long? countIsNational = documentList.Count(x => x.NationalProvider);
            long? countIsNotNational = documentList.Count(x => x.NationalProvider == false);
       
            res.Add(isNational, countIsNational);
            res.Add(isNotNational, countIsNotNational);

            return res;
        }
    }
}