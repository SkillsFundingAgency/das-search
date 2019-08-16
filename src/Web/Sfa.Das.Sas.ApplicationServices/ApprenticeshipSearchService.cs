using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Collections.Generic;
    using Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Settings;

    public sealed class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly IApprenticeshipSearchProvider _searchProvider;
        private readonly ILogger<ApprenticeshipSearchService> _logger;
        private readonly PaginationSettings _paginationSettings;

        public ApprenticeshipSearchService(
            IApprenticeshipSearchProvider searchProvider,
            ILogger<ApprenticeshipSearchService> logger,
            IOptions<PaginationSettings> paginationSettings)
        {
            _searchProvider = searchProvider;
            _logger = logger;
            _paginationSettings = paginationSettings.Value;
        }

        public async Task<ApprenticeshipSearchResults> SearchByKeyword(string keywords, int page, int take, int order, List<int> selectedLevels)
        {
            var takeElements = take == 0 ? _paginationSettings.DefaultResultsAmount : take;
            var results = await _searchProvider.SearchByKeyword(keywords, page, takeElements, order, selectedLevels);

             _logger.LogInformation(
                "Apprenticeship Keyword Search {ApprenticeshipSearch}",
                new ApprenticeshipSearchLogEntry { TotalHits = results?.TotalResults ?? -1, Keywords = keywords?.Split(' ') ?? new[] { "[empty]" } });

            return results;
        }
    }
}