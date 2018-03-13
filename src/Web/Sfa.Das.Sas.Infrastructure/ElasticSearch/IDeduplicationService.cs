using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public interface IDeduplicationService
    {
        IEnumerable<T> DedupeAtYourLocationOnlyDocuments<T>(IEnumerable<T> documents) 
            where T : IApprenticeshipProviderSearchResultsItem;
        Dictionary<string,long?> RetrieveTrainingOptionsAggregationElements<T>(IEnumerable<T> documents) 
            where T : IApprenticeshipProviderSearchResultsItem;

        Dictionary<string, long?> RetrieveNationalProvidersAggregationElements<T>(IEnumerable<T> documents)
            where T : IApprenticeshipProviderSearchResultsItem;
    }
}