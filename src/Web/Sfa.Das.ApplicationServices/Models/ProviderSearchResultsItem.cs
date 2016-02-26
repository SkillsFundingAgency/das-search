namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    using Eds.Das.Core.Domain.Model;

    public sealed class ProviderSearchResultsItem
    {
        public int Id { get; set; }

        public string UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string VenueName { get; set; }

        public string PostCode { get; set; }

        public Coordinate Coordinate { get; set; }

        public int Radius { get; set; }

        public double Distance { get; set; }

        public List<int> StandardsId { get; set; }
    }
}