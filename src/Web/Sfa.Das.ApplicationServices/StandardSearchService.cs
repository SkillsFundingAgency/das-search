namespace Sfa.Das.ApplicationServices
{
    using Sfa.Das.ApplicationServices.Models;

    public class StandardSearchService : IStandardSearchService
    {
        private readonly ISearchProvider searchProvider;

        public StandardSearchService(ISearchProvider searchProvider)
        {
            this.searchProvider = searchProvider;
        }

        public StandardSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            take = take == 0 ? 1000 : take;
            var results = this.searchProvider.SearchByKeyword(keywords, skip, take);

            return results;
        }
    }
}