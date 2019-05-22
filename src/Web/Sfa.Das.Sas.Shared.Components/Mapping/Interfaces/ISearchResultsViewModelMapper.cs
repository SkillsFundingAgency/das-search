using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public interface ISearchResultsViewModelMapper
    {
        SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel> Map(ProviderSearchResponse item, TrainingProviderSearchViewModel searchQueryModel);

    }
}
