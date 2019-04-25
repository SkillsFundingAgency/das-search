using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class FatSearchResultsItemViewModelMapper : IFatSearchResultsItemViewModelMapper
    {
        public FatSearchResultsItemViewModel Map(ApprenticeshipSearchResultsItem source, ICssClasses cssClasses)
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
