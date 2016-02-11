using System.Collections.Generic;

namespace Sfa.Eds.Das.Core.Models
{

    public sealed class ProviderSearchResultsItem
    {
        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public List<int> StandardsId { get; set; }
    }
}