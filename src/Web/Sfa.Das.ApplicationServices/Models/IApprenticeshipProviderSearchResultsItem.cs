using System.Collections.Generic;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Das.ApplicationServices.Models
{
    public interface IApprenticeshipProviderSearchResultsItem
    {
        Address Address { get; set; }
        string ContactUsUrl { get; set; }
        List<string> DeliveryModes { get; set; }
        double Distance { get; set; }
        string Email { get; set; }
        double? EmployerSatisfaction { get; set; }
        string Id { get; set; }
        double? LearnerSatisfaction { get; set; }
        int LocationId { get; set; }
        string LocationName { get; set; }
        string MarketingName { get; set; }
        string ProviderMarketingInfo { get; set; }
        string ApprenticeshipMarketingInfo { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        string ApprenticeshipInfoUrl { get; set; }
        string UkPrn { get; set; }
        string Website { get; set; }
    }
}