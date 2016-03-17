using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    public sealed class FrameworkInformation
    {
        public int FrameworkCode { get; set; }

        public int Level { get; set; }

        public int PathwayCode { get; set; }

        public string FrameworkInfoUrl { get; set; }

        public ContactInformation FrameworkContact { get; set; }

        public string MarketingInfo { get; set; }

        public IEnumerable<DeliveryInformation> DeliveryLocations { get; set; }
    }
}
