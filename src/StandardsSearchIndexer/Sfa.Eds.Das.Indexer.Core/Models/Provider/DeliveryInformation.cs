using System.Collections.Generic;

namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    public sealed class DeliveryInformation
    {
        public Location DeliveryLocation { get; set; }

        public IEnumerable<ModesOfDelivery> DeliveryModes { get; set; }

        public int Radius { get; set; }
    }
}
