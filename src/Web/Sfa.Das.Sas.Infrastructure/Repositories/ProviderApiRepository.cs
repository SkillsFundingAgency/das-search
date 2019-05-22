using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.FatApi.Client.Api;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using SFA.DAS.Providers.Api.Client;

    public class ProviderApiRepository : IGetProviderDetails, IProviderSearchProvider
    {

        private readonly IProviderApiClient _providerApiClient;
        private readonly IProvidersVApi _providersV3Api;
        private readonly ISearchResultsMapping _searchResultsMapping;

        public ProviderApiRepository(IProviderApiClient providerApiClient, IProvidersVApi providersV3Api, ISearchResultsMapping searchResultsMapping)
        {
            _providerApiClient = providerApiClient;
            _providersV3Api = providersV3Api;
            _searchResultsMapping = searchResultsMapping;
        }

        public async Task<Provider> GetProviderDetails(long ukPrn)
        {
            var result = await _providerApiClient.GetAsync(ukPrn);
            return result;
        }

        public IEnumerable<ProviderSummary> GetAllProviders()
        {
            var res = _providerApiClient.FindAll();
            return res;
        }

        public async Task<ApprenticeshipTrainingSummary> GetApprenticeshipTrainingSummary(long ukprn, int pageNumber)
        {
            return await _providerApiClient.GetActiveApprenticeshipTrainingByProviderAsync(ukprn, pageNumber);
        }

        public async Task<SearchResult<ProviderSearchResultItem>> SearchProvidersByLocation(string apprenticeshipId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter)
        {
            var result = await _providersV3Api.GetByApprenticeshipIdAndLocationAsync(apprenticeshipId, coordinates.Lat, coordinates.Lon, page, take, filter.HasNonLevyContract, false, "0,1,2");
            

            return _searchResultsMapping.Map(result);
        }
    }
}
