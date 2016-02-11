using System.Collections.Generic;
using Nest;

namespace Sfa.Eds.Indexer.Indexer.Infrastructure.Models
{
    public class Provider
    {
        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public List<int> StandardsId { get; set; }
        //public EnvelopeGeoShape Circle { get; set; }
    }
}
