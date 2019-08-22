using System;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.FatApi.Client.Api;
using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
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
        private readonly ISearchVApi _searchV3Api;
        private readonly ISearchResultsMapping _searchResultsMapping;
        private readonly IProviderNameSearchMapping _providerNameSearchMapping;
        private readonly ILog _logger;

        public ProviderApiRepository(IProviderApiClient providerApiClient, IProvidersVApi providersV3Api, ISearchResultsMapping searchResultsMapping, ILog logger, ISearchVApi searchV3Api, IProviderNameSearchMapping providerNameSearchMapping)
        {
            _providerApiClient = providerApiClient;
            _providersV3Api = providersV3Api;
            _searchResultsMapping = searchResultsMapping;
            _logger = logger;
            _searchV3Api = searchV3Api;
            _providerNameSearchMapping = providerNameSearchMapping;
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
            var result = await _providersV3Api.GetByApprenticeshipIdAndLocationAsync(apprenticeshipId, coordinates.Lat, coordinates.Lon, page, take, filter.HasNonLevyContract, filter.ShowNationalOnly, string.Join(",", filter.DeliveryModes));
            

            return _searchResultsMapping.Map(result);
        }

        public async Task<ProviderNameSearchResultsAndPagination> SearchProviderNameAndAliases(string searchTerm, int page, int pageSize)
        {
            var results = new ProviderNameSearchResultsAndPagination();

            if (searchTerm.Length < 3)
            {
                _logger.Info(
                    $"Search term causing SearchTermTooShort: [{searchTerm}]");

                results.SearchTerm = searchTerm;
                results.HasError = true;
                results.ResponseCode = ProviderNameSearchResponseCodes.SearchTermTooShort;

                return results;
            }

            _logger.Info(
                $"Provider Name Search started: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{pageSize}]");

            try
            {
                var apiResults = await _searchV3Api.SearchProviderNameAsync(searchTerm, page, pageSize);

                results = _providerNameSearchMapping.Map(apiResults, searchTerm);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Provider Name Search error: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{pageSize}]");
                results.ResponseCode = ProviderNameSearchResponseCodes.SearchFailed;
                results.HasError = true;
                return results;
            }

            _logger.Info(
                $"Provider Name Search complete: SearchTerm: [{searchTerm}], Page: [{results.ActualPage}], Page Size: [{pageSize}], Total Results: [{results.TotalResults}]");

            return results;
        }
    }
}
