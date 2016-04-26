using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices
{
    public interface ISearchProvider
    {
        ApprenticeshipSearchResults SearchByKeyword(string keywords, int skip, int take);
        SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int standardId, Coordinate testCoordinates);
        SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int standardId, Coordinate testCoordinates);
    }
}
