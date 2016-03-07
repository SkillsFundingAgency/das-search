using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models
{
    public class StandardProvider
    {
        public int StandardCode { get; set; }

        public string MarketingInfo { get; set; }

        public string StandardInfoUrl { get; set; }

        public Contact Contact { get; set; }

        public IEnumerable<LocationBasic> Locations { get; set; }
    }
}
