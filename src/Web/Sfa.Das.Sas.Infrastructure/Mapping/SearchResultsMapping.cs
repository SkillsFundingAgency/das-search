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

            var item = new SearchResult<ProviderSearchResultItem>();
            item.Total = document.TotalResults;
            item.Hits = document.Results?.Select(s => _providerSearchResultsMapper.Map(s));

            return item;
        }


    }
}