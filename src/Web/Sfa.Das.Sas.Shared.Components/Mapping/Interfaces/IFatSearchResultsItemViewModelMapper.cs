using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public interface IFatSearchResultsItemViewModelMapper
    {
        FatSearchResultsItemViewModel Map(ApprenticeshipSearchResultsItem item,ICssClasses cssClasses);
    }
}
