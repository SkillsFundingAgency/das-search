using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.Core.Domain.Model;

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
            item.LocationName = document.Location.LocationName;
            item.Address = new Address()
            {
                Address1 = document.Location.Address.Street,
                Town = document.Location.Address.Town,
                Postcode = document.Location.Address.Postcode,
                County = document.Location.Address.Primary

            };
            item.IsHigherEducationInstitute = document.IsHigherEducationInstitute;
            return item;
        }
    }
}