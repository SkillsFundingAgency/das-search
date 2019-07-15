using System.Linq;
using Sfa.Das.FatApi.Client.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using Sfa.Das.Sas.ApplicationServices.Models;
    public class ProviderSearchResultsMapper : IProviderSearchResultsMapper
    {

        public ProviderSearchResultItem Map(SFADASApprenticeshipsApiTypesV3ProviderSearchResultItem document)
        {
            if (document == null)
            {
                return null;
            }

            var item = new ProviderSearchResultItem();

            item.ProviderName = document.ProviderName;
            item.EmployerSatisfaction = document.EmployerSatisfaction;
            item.LearnerSatisfaction = document.LearnerSatisfaction;
            item.NationalProvider = document.NationalProvider;
            item.Ukprn = document.Ukprn;
            item.OverallAchievementRate = document.OverallAchievementRate;
            item.DeliveryModes = document.DeliveryModes;
            item.Distance = document.Distance;
            item.NationalProvider = document.NationalProvider;
            item.LocationId = document.Location.LocationId;
            return item;
        }
    }
}