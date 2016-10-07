using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices
{
    public interface IProviderLocationSearchProvider
    {
        SearchResult<StandardProviderSearchResultsItem> SearchStandardProviders(string standardId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter);

        SearchResult<FrameworkProviderSearchResultsItem> SearchFrameworkProviders(string frameworkId, Coordinate coordinates, int page, int take, ProviderSearchFilter filter);
    }
}