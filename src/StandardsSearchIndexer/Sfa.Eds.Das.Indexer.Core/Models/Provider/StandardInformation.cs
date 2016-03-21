namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    using System.Collections.Generic;

    public sealed class StandardInformation
    {
        public int StandardCode { get; set; }

        public ContactInformation StandardContact { get; set; }

        public string MarketingInfo { get; set; }

        public string StandardInfoUrl { get; set; }

        public IEnumerable<DeliveryInformation> DeliveryLocations { get; set; }
    }
}