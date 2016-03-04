namespace Sfa.Eds.Das.Indexer.Core.Models.ProviderImport
{
    using System.Collections.Generic;

    public class LocationRef
    {
        public IEnumerable<string> DeliveryModes { get; set; }
        public int ID { get; set; }
        public string MarketingInfo { get; set; }
        public int Radius { get; set; }
        public string StandardInfoUrl { get; set; }
    }
}