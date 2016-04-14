using Nest;

namespace Sfa.Infrastructure.Elasticsearch.Models
{
    using Newtonsoft.Json;

    public sealed class StandardProvider : IProviderAppreticeshipDocument
    {
        public int StandardCode { get; set; }
        public int StandardsId { get; set; }
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string ProviderMarketingInfo { get; set; }
        public string ApprenticeshipMarketingInfo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactUsUrl { get; set; }
        public string StandardInfoUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public double? LearnerSatisfaction { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public double? EmployerSatisfaction { get; set; }

        public string[] DeliveryModes { get; set; }
        public string Website { get; set; }
        public Address Address { get; set; }
        [GeoPoint]
        public GeoCoordinate LocationPoint { get; set; }
        [GeoShape]
        public CircleGeoShape Location { get; set; }
    }
}
