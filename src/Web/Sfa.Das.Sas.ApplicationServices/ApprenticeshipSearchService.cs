using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly IApprenticeshipSearchProvider _searchProvider;
        private readonly ILog _logger;
        private readonly IPaginationSettings _paginationSettings;

        public ApprenticeshipSearchService(IApprenticeshipSearchProvider searchProvider, ILog logger, IPaginationSettings paginationSettings)
        {
            _searchProvider = searchProvider;
            _logger = logger;
            _paginationSettings = paginationSettings;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, int order, List<int> selectedLevels)
        {
            _logger.Info($"Apprenticeship Keyword Search: {keywords}", new Dictionary<string, object> { { "keywords", keywords?.Split(' ') ?? new string[] { "[empty]" } } });

            var takeElements = take == 0 ? _paginationSettings.DefaultResultsAmount : take;

            var results = _searchProvider.SearchByKeyword(keywords, page, takeElements, order, selectedLevels);

            return results;
        }
    }
}