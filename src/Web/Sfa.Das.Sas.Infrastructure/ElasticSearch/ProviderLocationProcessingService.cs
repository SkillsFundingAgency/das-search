using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public class ProviderLocationProcessingService : IProviderLocationProcessingService
    {
        public Dictionary<string, long?> RetrieveTrainingOptionsAggregationElements<T>(IEnumerable<T> documents)
            where T : IApprenticeshipProviderSearchResultsItem
        {
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
            const string isNational = "1";
            const string isNotNational = "0";
            var res = new Dictionary<string, long?>();
            var documentList = documents.ToList();

            long? countIsNational = documentList.Count(x => x.NationalProvider);
            long? countIsNotNational = documentList.Count(x => x.NationalProvider == false);

            res.Add(isNational, countIsNational);
            res.Add(isNotNational, countIsNotNational);

            return res;
        }

        public List<T> FilterProviderSearchResults<T>(List<T> documents, ProviderSearchFilter filter)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            if (filter.SearchOption == ProviderFilterOptions.ApprenticeshipLocationWithNationalProviderOnly)
            {
                documents = documents.Where(x => x.NationalProvider).ToList();
            }

            var isAll = filter.DeliveryModes == null;
            var is100PercentEmxployer = filter.DeliveryModes != null && filter.DeliveryModes.Contains("100percentemployer");
            var isDayRelease = filter.DeliveryModes != null && filter.DeliveryModes.Contains("dayrelease");
            var isBlockRelease = filter.DeliveryModes != null && filter.DeliveryModes.Contains("blockrelease");

            documents = documents
                .Where(x => (isAll
                             || (is100PercentEmxployer && x.DeliveryModes.Contains("100PercentEmployer"))
                             || (isDayRelease && x.DeliveryModes.Contains("DayRelease"))
                             || (isBlockRelease && x.DeliveryModes.Contains("BlockRelease"))))
                .ToList();
            return documents;
        }

        public IEnumerable<T> CastDocumentsToMatchingResultsItemType<T>(List<IApprenticeshipProviderSearchResultsItem> documents)
            where T : class, IApprenticeshipProviderSearchResultsItem
        {
            IEnumerable<T> documentsRecast = null;

            if (typeof(T) == typeof(FrameworkProviderSearchResultsItem))
            {
                documentsRecast = (IEnumerable<T>)documents.Select(x => (FrameworkProviderSearchResultsItem)x).ToList();
            }

            if (typeof(T) == typeof(StandardProviderSearchResultsItem))
            {
                documentsRecast = (IEnumerable<T>)documents.Select(x => (StandardProviderSearchResultsItem)x).ToList();
            }

            return documentsRecast;
        }
    }
}
