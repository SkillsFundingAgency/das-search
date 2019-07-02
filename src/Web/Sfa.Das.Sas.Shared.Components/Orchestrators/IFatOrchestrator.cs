using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public interface IFatOrchestrator
    {
        FatSearchResultsViewModel GetSearchResults(SearchQueryViewModel searchQueryModel);
        FatSearchFilterViewModel GetSearchFilters(SearchQueryViewModel searchQueryModel);
    }
}