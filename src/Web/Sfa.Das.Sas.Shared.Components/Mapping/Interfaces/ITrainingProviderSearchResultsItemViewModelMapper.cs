using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public interface ITrainingProviderSearchResultsItemViewModelMapper
    {
        TrainingProviderSearchResultsItem Map(ProviderSearchResultItem item);
    }
}
