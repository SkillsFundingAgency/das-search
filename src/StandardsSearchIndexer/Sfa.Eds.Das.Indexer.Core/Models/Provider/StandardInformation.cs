using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    public sealed class StandardInformation : IApprenticeshipInformation
    {
        public int Code { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public string MarketingInfo { get; set; }

        public string InfoUrl { get; set; }

        public IEnumerable<DeliveryInformation> DeliveryLocations { get; set; }
    }
}
