namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    using System.Collections.Generic;

    public sealed class DeliveryInformation
    {
        public Location DeliveryLocation { get; set; }

        public IEnumerable<ModesOfDelivery> DeliveryModes { get; set; }

        public int Radius { get; set; }
    }
}