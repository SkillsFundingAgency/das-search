using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Collections.Generic;

    public interface IProviderLocationSearchProvider
    {
        SearchResult<StandardProviderSearchResultsItem> SearchStandardProviders(int standardId, Coordinate coordinates, int page, int take, SearchFilter filter);

        SearchResult<FrameworkProviderSearchResultsItem> SearchFrameworkProviders(int frameworkId, Coordinate coordinates, int page, int take, SearchFilter filter);
    }
}