namespace Sfa.Eds.Das.Core.Interfaces.Search
{
    using Models;

    public interface ISearchService
    {
        SearchResults SearchByKeyword(string keywords, int skip, int take);

        SearchResultsItem GetStandardItem(string id);
    }
}
