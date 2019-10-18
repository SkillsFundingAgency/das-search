using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels.Apprenticeship;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class ApprenticeshipBasketItemViewModelMapper : IApprenticeshipBasketItemViewModelMapper
    {
        public ApprenticeshipBasketItemViewModel Map(ApprenticeshipFavouriteRead source)
        {
            var item = new ApprenticeshipBasketItemViewModel()
            {
                Id = source.ApprenticeshipId,
                Title = source.Title,
                Level = source.Level,
                Duration = source.Duration,
                TrainingProvider = source.Providers.Select(s => new TrainingProviderSearchResultsItem(){Ukprn = s.Ukprn, Name = s.Name}).ToList(),
                ApprenticeshipType = source.ApprenticeshipType,
                EffectiveTo = source.EffectiveTo
            };
            return item;
        }
    }
}