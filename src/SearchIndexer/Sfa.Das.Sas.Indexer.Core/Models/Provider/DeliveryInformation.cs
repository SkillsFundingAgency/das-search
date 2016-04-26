using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.Core.Models.Provider
{
    public sealed class DeliveryInformation
    {
        public Location DeliveryLocation { get; set; }

        public IEnumerable<ModesOfDelivery> DeliveryModes { get; set; }

        public int Radius { get; set; }
    }
}