namespace Sfa.Eds.Das.Core.Interfaces.Search
{
    using Models;

    public interface ISearchService
    {
        StandardSearchResults SearchByKeyword(string keywords, int skip, int take);

        StandardSearchResultsItem GetStandardItem(string id);
    }
}
