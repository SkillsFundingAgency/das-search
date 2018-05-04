using System;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using Settings;
    using SFA.DAS.NLog.Logger;

    public class ProviderNameSearchService : IProviderNameSearchService
    {
        private readonly IPaginationSettings _paginationSettings;
        private readonly IProviderNameSearchProvider _searchProviderName;
        private readonly ILog _logger;
        public ProviderNameSearchService(IPaginationSettings paginationSettings, IProviderNameSearchProvider searchProviderName, ILog logger)
        {
            _paginationSettings = paginationSettings;
            _searchProviderName = searchProviderName;
            _logger = logger;
        }

        public async Task<ProviderNameSearchResultsAndPagination> SearchProviderNameAndAliases(string searchTerm, int page)
        {
            var results = new ProviderNameSearchResultsAndPagination();

            var take = _paginationSettings.DefaultResultsAmount;

            _logger.Info(
                $"Provider Name Search started: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{take}]");

            try
            {
                results = await _searchProviderName.SearchByTerm(searchTerm, page, take);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Provider Name Search eror: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{take}]");
                results.ResponseCode = ProviderNameSearchResponseCodes.SearchFailed;
                results.HasError = true;
                _logger.Error(e, $"Provider Name Search failed: SearchTerm: [{searchTerm}], Page: [{page}], Page Size: [{take}]");
            }

            _logger.Info(
                $"Provider Name Search complete: SearchTerm: [{searchTerm}], Page: [{results.ActualPage}], Page Size: [{take}], Total Results: [{results.TotalResults}]");

            return results;
        }
    }
}
