using System;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class ProviderMapping : IProviderMapping
    {
        ApprenticeshipDetails IProviderMapping.MapToProvider(StandardProviderSearchResultsItem item)
        {
            if (item == null)
            {
                return null;
            }

            var details = MapFromInterface(item);
            details.Product.Apprenticeship.Code = item.StandardCode;

            return details;
        }

        public ApprenticeshipDetails MapToProvider(FrameworkProviderSearchResultsItem item)
        {
            var details = MapFromInterface(item);

            details.Product.Apprenticeship.Code = int.Parse(item.FrameworkId);

            return details;
        }

        private static ApprenticeshipDetails MapFromInterface(IApprenticeshipProviderSearchResultsItem item)
        {
            var providerIdString = item.Id
                                       .Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                                       .First();

            var providerId = int.Parse(providerIdString);

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
                    LearnerSatisfaction = item.LearnerSatisfaction * 10
                },
                Location = new Location
                {
                    LocationId = item.LocationId,
                    LocationName = item.LocationName,
                    Address = item.Address,
                    Distance = item.Distance
                },
                Provider = new Provider
                {
                    Id = providerId,
                    Name = item.Name,
                    UkPrn = item.UkPrn,
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