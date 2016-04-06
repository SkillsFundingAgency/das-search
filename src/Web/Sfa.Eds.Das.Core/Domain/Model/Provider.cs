namespace Sfa.Eds.Das.Core.Domain.Model
{
    using System.Collections.Generic;

    public sealed class Provider
    {
        public string Id { get; set; }

        public string UkPrn { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public List<string> DeliveryModes { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public Address Address { get; set; }

        public double Distance { get; set; }

        public double? EmployerSatisfaction { get; set; }

        public double? LearnerSatisfaction { get; set; }

        public ApprenticeshipBasic Apprenticeship { get; set; }
    }
}
