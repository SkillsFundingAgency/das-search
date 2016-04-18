namespace Sfa.Das.ApplicationServices
{
    using Sfa.Das.ApplicationServices.Models;

    public sealed class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly ISearchProvider _searchProvider;

        public ApprenticeshipSearchService(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public ApprenticeshipSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            var takeElements = take == 0 ? 1000 : take;
            var results = _searchProvider.SearchByKeyword(keywords, skip, takeElements);

            return results;
        }
    }
}