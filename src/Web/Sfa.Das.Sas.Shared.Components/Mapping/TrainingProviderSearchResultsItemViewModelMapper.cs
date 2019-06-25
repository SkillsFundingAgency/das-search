using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class TrainingProviderSearchResultsItemViewModelMapper : ITrainingProviderSearchResultsItemViewModelMapper
    {

        public TrainingProviderSearchResultsItem Map(ProviderSearchResultItem source)
        {
            var item = new TrainingProviderSearchResultsItem()
            {
                Ukprn = source.Ukprn,
                Distance = source.Distance,
                EmployerSatisfaction = source.EmployerSatisfaction,
                LearnerSatisfaction = source.LearnerSatisfaction,
                NationalProvider = source.NationalProvider,
                OverallAchievementRate = source.OverallAchievementRate,
                Name = source.ProviderName,
                DeliveryModes = source.DeliveryModes,
                LocationId = source.LocationId
            };
            return item;
        }
    }
}
