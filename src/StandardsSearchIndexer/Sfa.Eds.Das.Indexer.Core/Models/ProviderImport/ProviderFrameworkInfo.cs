namespace Sfa.Eds.Das.Indexer.Core.Models.ProviderImport
{
    using System.Collections.Generic;

    public class ProviderFrameworkInfo
    {
        public Contact Contact { get; set; }

        public int FrameworkCode { get; set; }

        public int Level { get; set; }

        public IEnumerable<LocationRef> Locations { get; set; }

        public int PathwayCode { get; set; }
    }
}