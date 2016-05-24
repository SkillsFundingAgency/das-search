using System;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class ProviderMapping : IProviderMapping
    {
        ProviderCourse IProviderMapping.MapToProvider(StandardProviderSearchResultsItem item)
        {
            if (item == null)
            {
                return null;
            }

            var document = MapFromInterface(item);
            document.Apprenticeship.Code = item.StandardCode;
            return document;
        }

        public ProviderCourse MapToProvider(FrameworkProviderSearchResultsItem item)
        {
            var document = MapFromInterface(item);

            document.Apprenticeship.Code = Convert.ToInt32(item.FrameworkId);
            return document;
        }

        private static ProviderCourse MapFromInterface(IApprenticeshipProviderSearchResultsItem item)
        {
            var providerId = item.Id.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                                    .FirstOrDefault();

            return new ProviderCourse
                       {
                           ProviderId = providerId,
                           Address = item.Address,
                           DeliveryModes = item.DeliveryModes,
                           UkPrn = item.UkPrn,
                           ContactInformation =
                               new ContactInformation
                                   {
                                       Phone = item.Phone,
                                       Email = item.Email,
                                       Website = item.Website,
                                       ContactUsUrl = item.ContactUsUrl
                                   },
                           Apprenticeship =
                               new ApprenticeshipBasic
                                   {
                                       ApprenticeshipInfoUrl = item.ApprenticeshipInfoUrl,
                                       ApprenticeshipMarketingInfo =
                                           item.ApprenticeshipMarketingInfo
                                   },
                           LearnerSatisfaction = item.LearnerSatisfaction * 10,
                           EmployerSatisfaction = item.EmployerSatisfaction * 10,
                           Location =
                               new Location { LocationName = item.LocationName, LocationId = item.LocationId },
                           Name = item.Name,
                           ProviderMarketingInfo = item.ProviderMarketingInfo,
                           Distance = item.Distance
                       };
        }
    }
}