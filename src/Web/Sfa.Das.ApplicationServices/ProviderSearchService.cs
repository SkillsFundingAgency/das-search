using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.ApplicationServices;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Das.ApplicationServices.Exceptions;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Das.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly IStandardRepository _standardRepository;
        private readonly ISearchProvider _searchProvider;
        private readonly ILookupLocations _postCodeLookup;
        private readonly ILog _logger;

        public ProviderSearchService(ISearchProvider searchProvider, IStandardRepository standardRepository, ILookupLocations postcodeLookup, ILog logger)
        {
            _searchProvider = searchProvider;
            _standardRepository = standardRepository;
            _postCodeLookup = postcodeLookup;
            _logger = logger;
        }

        public async Task<ProviderSearchResults> SearchByPostCode(int standardId, string postCode)
        {
            if (string.IsNullOrEmpty(postCode))
            {
                return new ProviderSearchResults { StandardId = standardId, PostCodeMissing = true };
            }

            string standardName = string.Empty;

            try
            {
                standardName = _standardRepository.GetById(standardId)?.Title;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                if (coordinates == null)
                {
                    return new ProviderSearchResults
                    {
                        TotalResults = 0,
                        StandardId = standardId,
                        StandardName = standardName,
                        PostCode = postCode,
                        Hits = new ProviderSearchResultsItem[0],
                        HasError = false
                    };
                }
                else
                {
                    var searchResults = _searchProvider.SearchByLocation(standardId, coordinates);

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
            catch (SearchException ex)
            {
                _logger.Error(ex, "Search for Provider failed.");

                return new ProviderSearchResults
                {
                    TotalResults = 0,
                    StandardId = standardId,
                    StandardName = standardName,
                    PostCode = postCode,
                    Hits = new ProviderSearchResultsItem[0],
                    HasError = true
                };
            }
        }
    }
}
