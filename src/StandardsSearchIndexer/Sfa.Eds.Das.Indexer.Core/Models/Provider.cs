namespace Sfa.Eds.Das.Indexer.Core.Models
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.Common.Models;

    public class Provider : IIndexEntry
    {
        public int ProviderId { get; set; }

        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public List<int> StandardsId { get; set; }
    }
}
