namespace Sfa.Eds.Das.Core.Interfaces.Search
{
    using Models;

    public interface ISearchService
    {
        SearchResults SearchByKeyword(string keywords);

        SearchResultsItem GetStandardItem(string id);
    }
}
