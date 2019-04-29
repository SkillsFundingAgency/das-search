using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class FatSearchResultsItemViewModelMapper : IFatSearchResultsItemViewModelMapper
    {
        public FatSearchResultsItemViewModel Map(ApprenticeshipSearchResultsItem source)
        {
            var item = new FatSearchResultsItemViewModel()
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Duration = source.Duration,
                EffectiveTo = source.EffectiveTo,
                ApprenticeshipType = source.ApprenticeshipType
            };
            return item;
        }
    }
}
