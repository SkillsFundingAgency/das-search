namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    using System.Collections.Generic;

    public sealed class StandardInformation : IApprenticeshipInformation
    {
        public int Code { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public string MarketingInfo { get; set; }

        public string InfoUrl { get; set; }

        public IEnumerable<DeliveryInformation> DeliveryLocations { get; set; }
    }
}