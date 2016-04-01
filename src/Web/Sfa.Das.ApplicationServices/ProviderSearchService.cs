using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Das.ApplicationServices.Exceptions;

namespace Sfa.Das.ApplicationServices
{
    public sealed class ProviderSearchService : IProviderSearchService
    {
        private readonly IApprenticeshipRepository _apprenticeshipRepository;
        private readonly ISearchProvider _searchProvider;
        private readonly ILookupLocations _postCodeLookup;
        private readonly ILog _logger;

        public ProviderSearchService(ISearchProvider searchProvider, IApprenticeshipRepository apprenticeshipRepository, ILookupLocations postcodeLookup, ILog logger)
        {
            _searchProvider = searchProvider;
            _apprenticeshipRepository = apprenticeshipRepository;
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
                standardName = _apprenticeshipRepository.GetStandardById(standardId)?.Title;

                var coordinates = await _postCodeLookup.GetLatLongFromPostCode(postCode);

                if (coordinates == null)
                {
                    return new ProviderSearchResults
                    {
                        TotalResults = 0,
                        StandardId = standardId,
                        StandardName = standardName,
                        PostCode = postCode,
                        Hits = new IApprenticeshipProviderSearchResultsItem[0],
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
                    Hits = new IApprenticeshipProviderSearchResultsItem[0],
                    HasError = true
                };
            }
        }
    }
}
