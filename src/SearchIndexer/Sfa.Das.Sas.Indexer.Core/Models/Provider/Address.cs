namespace Sfa.Das.Sas.Indexer.Core.Models.Provider
{
    public sealed class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string County { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public Coordinate GeoPoint { get; set; }
    }
}
