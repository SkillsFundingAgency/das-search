using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices
{
    public sealed class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ILog _logger;

        public ApprenticeshipSearchService(ISearchProvider searchProvider, ILog logger)
        {
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            _logger.Info($"Apprenticeship Keyword Search: {keywords}", new Dictionary<string, object> { { "keywords", keywords?.Split(' ') ?? new string[] { "[empty]" } } });

            var takeElements = take == 0 ? 100 : take;
            var results = _searchProvider.SearchByKeyword(keywords, skip, takeElements);

            return results;
        }
    }
}