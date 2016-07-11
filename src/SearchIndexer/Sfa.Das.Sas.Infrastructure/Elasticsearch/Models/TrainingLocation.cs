namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using Nest;

    public class TrainingLocation
    {
        public int LocationId { get; set; }

        public string LocationName { get; set; }

        [GeoShape]
        public CircleGeoShape Location { get; set; }

        public Address Address { get; set; }

        [GeoPoint]
        public GeoCoordinate LocationPoint { get; set; }
    }
}