using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models
{
    public class LocationBasic
    {
        public int Id { get; set; }

        public IEnumerable<string> DeliveryModes { get; set; }

        public int Radius { get; set; }
    }
}
