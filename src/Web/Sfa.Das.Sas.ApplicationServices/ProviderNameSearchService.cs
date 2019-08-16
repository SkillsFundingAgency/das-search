using System;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Settings;

    public class ProviderNameSearchService : IProviderNameSearchService
    {
        private readonly PaginationSettings _paginationSettings;
        private readonly IProviderSearchProvider _searchProviderName;
        private readonly ILogger<ProviderNameSearchService> _logger;
        public ProviderNameSearchService(IOptions<PaginationSettings> paginationSettings, IProviderSearchProvider searchProviderName, ILogger<ProviderNameSearchService> logger)
        {
            _paginationSettings = paginationSettings.Value;
            _searchProviderName = searchProviderName;
            _logger = logger;
        }

        public async Task<ProviderNameSearchResultsAndPagination> SearchProviderNameAndAliases(string searchTerm, int page, int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = _paginationSettings.DefaultResultsAmount;
            }

            _logger.LogInformation($"Provider Name Search started: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{pageSize}]");

            try
            {
                var results = await _searchProviderName.SearchProviderNameAndAliases(searchTerm, page, pageSize.Value);

                _logger.LogInformation($"Provider Name Search complete: SearchTerm: [{searchTerm}], Page: [{results.ActualPage}], Page Size: [{pageSize}], Total Results: [{results.TotalResults}]");

                return results;
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Provider Name Search error: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{pageSize}]");
                return new ProviderNameSearchResultsAndPagination()
                {
                    HasError = true,
                    ResponseCode = ProviderNameSearchResponseCodes.SearchFailed
                };
            }
        }
    }
}
