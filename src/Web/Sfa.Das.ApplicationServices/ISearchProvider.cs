using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Das.ApplicationServices
{
    public interface ISearchProvider
    {
        StandardSearchResults SearchByKeyword(string keywords, int skip, int take);

        SearchResult<ProviderSearchResultsItem> SearchByLocation(int standardId, Coordinate testCoordinates);
    }
}
