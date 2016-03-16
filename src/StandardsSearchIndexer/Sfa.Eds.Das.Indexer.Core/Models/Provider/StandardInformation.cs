using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    public sealed class StandardInformation
    {
        public int StandardCode { get; set; }

        public ContactInformation StandardContact { get; set; }

        public string MarketingInfo { get; set; }

        public string StandardInfoUrl { get; set; }

        public IEnumerable<DeliveryInformation> DeliveryLocations { get; set; }
    }
}
