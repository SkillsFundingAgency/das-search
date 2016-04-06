using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Mapping
{
    public class ProviderMapping : IProviderMapping
    {
        public Provider MapToProvider(StandardProviderSearchResultsItem item)
        {
            return new Provider
            {
                Id = item.Id,
                Address = item.Address,
                DeliveryModes = item.DeliveryModes,
                UkPrn = item.UkPrn,
                ContactInformation = new ContactInformation
                {
                    Phone = item.Phone,
                    Email = item.Email,
                    Website = item.Website,
                    ContactUsUrl = item.ContactUsUrl,
                },
                Apprenticeship = new ApprenticeshipBasic
                {
                    ApprenticeshipInfoUrl = item.StandardInfoUrl,
                    Code = item.StandardCode,
                    ApprenticeshipMarketingInfo = item.ApprenticeshipMarketingInfo,
                },
                LearnerSatisfaction = item.LearnerSatisfaction * 10,
                EmployerSatisfaction = item.EmployerSatisfaction * 10,
                Location = new Location
                {
                    LocationName = item.LocationName,
                    LocationId = item.LocationId,
                },
                Name = item.Name,
                ProviderMarketingInfo = item.ProviderMarketingInfo,
                Distance = item.Distance
            };
        }
    }
}
