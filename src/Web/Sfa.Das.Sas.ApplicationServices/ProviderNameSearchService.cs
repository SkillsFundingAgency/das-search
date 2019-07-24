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
        private readonly IProviderSearchProvider _searchProviderName;
        private readonly ILog _logger;
        public ProviderNameSearchService(IPaginationSettings paginationSettings, IProviderSearchProvider searchProviderName, ILog logger)
        {
            _paginationSettings = paginationSettings;
            _searchProviderName = searchProviderName;
            _logger = logger;
        }

        public async Task<ProviderNameSearchResultsAndPagination> SearchProviderNameAndAliases(string searchTerm, int page)
        {
            return await _searchProviderName.SearchProviderNameAndAliases(searchTerm, page, page);

        }
    }
}
