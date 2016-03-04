using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models
{
    public class FrameworkProvider
    {
        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }

        public string MarketingInfo { get; set; }

        public string FrameworkInfoUrl { get; set; }

        public Contact Contact { get; set; }

        public IEnumerable<LocationBasic> Locations { get; set; }
    }
}
