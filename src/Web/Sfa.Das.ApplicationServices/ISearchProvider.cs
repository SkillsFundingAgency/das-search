using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Das.ApplicationServices
{
    public interface ISearchProvider
    {
        ApprenticeshipSearchResults SearchByKeyword(string keywords, int skip, int take);
        SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int standardId, Coordinate testCoordinates);
        SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int standardId, Coordinate testCoordinates);
    }
}
