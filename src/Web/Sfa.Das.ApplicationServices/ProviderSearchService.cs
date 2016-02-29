using System;
using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.ApplicationServices;
using Sfa.Eds.Das.Core.Domain.Services;

namespace Sfa.Das.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly IStandardRepository _standardRepository;
        private readonly ISearchProvider _searchProvider;
        private readonly ILookupLocations _postCodeLookup;

        public ProviderSearchService(ISearchProvider searchProvider, IStandardRepository standardRepository, ILookupLocations postcodeLookup)
        {
            _searchProvider = searchProvider;
            _standardRepository = standardRepository;
            _postCodeLookup = postcodeLookup;
        }

        public async Task<ProviderSearchResults> SearchByPostCode(int standardId, string postCode)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderSearchResults { StandardId = standardId, PostCodeMissing = true };
            }

            var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

            var searchResults = _searchProvider.SearchByLocation(standardId, coordinates);

            var standardName = _standardRepository.GetById(standardId)?.Title;

            var result = new ProviderSearchResults
            {
                TotalResults = searchResults.Total,
                StandardId = standardId,
                StandardName = standardName,
                PostCode = postCode,
                Hits = searchResults.Hits
            };

            return result;
        }
    }
}
