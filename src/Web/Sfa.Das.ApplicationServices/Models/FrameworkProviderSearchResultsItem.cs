namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Core.Domain.Model;

    public sealed class FrameworkProviderSearchResultsItem : IApprenticeshipProviderSearchResultsItem
    {
        public string Id { get; set; }

        public string UkPrn { get; set; }

        public string Name { get; set; }

        public string FrameworkId { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public string MarketingName { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public string ApprenticeshipMarketingInfo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactUsUrl { get; set; }

        public string StandardInfoUrl { get; set; }

        public List<string> DeliveryModes { get; set; }

        public string Website { get; set; }

        public Address Address { get; set; }

        public double Distance { get; set; }

        public double? EmployerSatisfaction { get; set; }

        public double? LearnerSatisfaction { get; set; }
    }
}