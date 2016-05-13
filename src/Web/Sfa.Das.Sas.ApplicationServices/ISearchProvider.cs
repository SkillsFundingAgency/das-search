using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Collections.Generic;

    public interface ISearchProvider
    {
        ApprenticeshipSearchResults SearchByKeyword(string keywords, int page, int take, List<string> selectedLevels);

        SearchResult<StandardProviderSearchResultsItem> SearchByStandardLocation(int standardId, Coordinate testCoordinates, IEnumerable<string> deliveryModes);

        SearchResult<FrameworkProviderSearchResultsItem> SearchByFrameworkLocation(int standardId, Coordinate testCoordinates, IEnumerable<string> deliveryModes);
    }
}