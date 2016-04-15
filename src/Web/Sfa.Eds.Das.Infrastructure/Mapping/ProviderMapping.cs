namespace Sfa.Eds.Das.Infrastructure.Mapping
{
    using System;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Model;

    public class ProviderMapping : IProviderMapping
    {
        Provider IProviderMapping.MapToProvider(StandardProviderSearchResultsItem item)
        {
            var document = MapFromInterface(item);
            document.Apprenticeship.Code = item.StandardCode;
            return document;
        }

        public Provider MapToProvider(FrameworkProviderSearchResultsItem item)
        {
            var document = MapFromInterface(item);

            document.Apprenticeship.Code = Convert.ToInt32(item.FrameworkId);
            return document;
        }

        private static Provider MapFromInterface(IApprenticeshipProviderSearchResultsItem item)
        {
            return new Provider
                       {
                           Id = item.Id,
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