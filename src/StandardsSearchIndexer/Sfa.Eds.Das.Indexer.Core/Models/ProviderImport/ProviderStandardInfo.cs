namespace Sfa.Eds.Das.Indexer.Core.Models.ProviderImport
{
    using System.Collections.Generic;

    public class ProviderStandardInfo
    {
        public Contact Contact { get; set; }
        public IEnumerable<LocationRef> Locations { get; set; }
        public string MarketingInfo { get; set; }
        public int StandardCode { get; set; }
        public string StandardInfoUrl { get; set; }
    }
}