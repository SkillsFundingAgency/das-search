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
        public Provider MapToProvider(StandardProviderSearchResultsItem item)
        {
            return new Provider
            {
                Id = item.Id,
                Address = item.Address,
                DeliveryModes = item.DeliveryModes,
                UkPrn = item.UkPrn,
                Phone = item.Phone,
                Email = item.Email,
                Website = item.Website,
                LearnerSatisfaction = item.LearnerSatisfaction * 10,
                EmployerSatisfaction = item.EmployerSatisfaction * 10,
                StandardInfoUrl = item.StandardInfoUrl,
                LocationName = item.LocationName,
                Name = item.Name,
                ContactUsUrl = item.ContactUsUrl,
                LocationId = item.LocationId,
                StandardCode = item.StandardCode,
                MarketingName = item.MarketingName,
                Distance = item.Distance
            };
        }
    }
}
