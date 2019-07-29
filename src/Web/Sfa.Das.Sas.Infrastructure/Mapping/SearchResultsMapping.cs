using System.Collections.Generic;
using System.Linq;
using Sfa.Das.FatApi.Client.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using Sfa.Das.Sas.ApplicationServices.Models;
    public class SearchResultsMapping : ISearchResultsMapping
    {
        private readonly IProviderSearchResultsMapper _providerSearchResultsMapper;

        public SearchResultsMapping( IProviderSearchResultsMapper providerSearchResultsMapper)
        {
            _providerSearchResultsMapper = providerSearchResultsMapper;
        }

        public SearchResult<ProviderSearchResultItem> Map(SFADASApprenticeshipsApiTypesV3ProviderApprenticeshipLocationSearchResult document)
        {
            if (document == null)
            {
                return null;
            }

            var result = new SearchResult<ProviderSearchResultItem>();
            result.Total = document.TotalResults;
            result.Hits = document.Results?.Select(s => _providerSearchResultsMapper.Map(s));

            result.NationalProvidersAggregation = document.NationalProvidersAggregation.ToDictionary(s => s.Key, s => s.Value as long?);
            result.TrainingOptionsAggregation = document.TrainingOptionsAggregation.ToDictionary(s => s.Key, s => s.Value as long?);

            return result;
        }


    }
}