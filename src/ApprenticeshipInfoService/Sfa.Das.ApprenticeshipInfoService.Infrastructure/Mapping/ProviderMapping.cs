using System.Linq;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping
{
    public class ProviderMapping : IProviderMapping
    {
        ApprenticeshipDetails IProviderMapping.MapToProvider(StandardProviderSearchResultsItem item, int locationId)
        {
            if (item == null)
            {
                return null;
            }

            var details = MapFromInterface(item, locationId);
            details.Product.Apprenticeship.Code = item.StandardCode;

            return details;
        }

        public ApprenticeshipDetails MapToProvider(FrameworkProviderSearchResultsItem item, int locationId)
        {
            var details = MapFromInterface(item, locationId);

            details.Product.Apprenticeship.Code = int.Parse(item.FrameworkId);

            return details;
        }

        private static ApprenticeshipDetails MapFromInterface(IApprenticeshipProviderSearchResultsItem item, int locationId)
        {
            var matchingLocation = item.TrainingLocations.Single(x => x.LocationId == locationId);

            return new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct
                {
                    Apprenticeship = new ApprenticeshipBasic
                    {
                        ApprenticeshipInfoUrl = item.ApprenticeshipInfoUrl,
                        ApprenticeshipMarketingInfo =
                                           item.ApprenticeshipMarketingInfo
                    },
                    DeliveryModes = item.DeliveryModes,
                    ProviderMarketingInfo = item.ProviderMarketingInfo,
                    EmployerSatisfaction = item.EmployerSatisfaction * 10,
                    LearnerSatisfaction = item.LearnerSatisfaction * 10,
                    AchievementRate = item.OverallAchievementRate,
                    NationalAchievementRate = item.NationalOverallAchievementRate,
                    OverallCohort = item.OverallCohort
                },
                Location = new Location
                {
                    LocationId = matchingLocation.LocationId,
                    LocationName = matchingLocation.LocationName,
                    Address = matchingLocation.Address
                },
                Provider = new Provider
                {
                    Name = item.ProviderName,
                    UkPrn = item.Ukprn,
                    NationalProvider = item.NationalProvider,
                    ContactInformation = new ContactInformation
                    {
                        Phone = item.Phone,
                        Email = item.Email,
                        Website = item.Website,
                        ContactUsUrl = item.ContactUsUrl
                    },
                }
            };
        }
    }
}
