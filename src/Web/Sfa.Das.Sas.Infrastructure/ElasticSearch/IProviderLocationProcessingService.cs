using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public interface IProviderLocationProcessingService
    {
        Dictionary<string, long?> RetrieveTrainingOptionsAggregationElements<T>(IEnumerable<T> documents)
            where T : IApprenticeshipProviderSearchResultsItem;

        Dictionary<string, long?> RetrieveNationalProvidersAggregationElements<T>(IEnumerable<T> documents)
            where T : IApprenticeshipProviderSearchResultsItem;

        List<T> FilterProviderSearchResults<T>(List<T> documentsDeduped, ProviderSearchFilter filter)
            where T : class, IApprenticeshipProviderSearchResultsItem;

        IEnumerable<T> CastDocumentsToMatchingResultsItemType<T>(List<IApprenticeshipProviderSearchResultsItem> documentsSubset)
            where T : class, IApprenticeshipProviderSearchResultsItem;

    }
}