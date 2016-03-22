using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Mapping
{
    public class ProviderMapping : IProviderMapping
    {
        public Provider MapToProvider(ProviderSearchResultsItem item)
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
                Standard = new StandardBasic
                {
                    StandardInfoUrl = item.StandardInfoUrl,
                    StandardCode = item.StandardCode,
                    ApprenticeshipMarketingInfo = item.ApprenticeshipMarketingInfo,
                },
                LearnerSatisfaction = item.LearnerSatisfaction * 10,
                EmployerSatisfaction = item.EmployerSatisfaction * 10,
                LocationName = item.LocationName,
                Name = item.Name,
                LocationId = item.LocationId,
                ProviderMarketingInfo = item.ProviderMarketingInfo,
                Distance = item.Distance
            };
        }
    }
}
