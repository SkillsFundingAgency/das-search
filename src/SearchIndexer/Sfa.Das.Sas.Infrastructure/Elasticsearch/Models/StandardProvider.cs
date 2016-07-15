using Nest;
using Newtonsoft.Json;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using System.Collections.Generic;

    public sealed class StandardProvider : IProviderAppreticeshipDocument
    {
        public int StandardCode { get; set; }

        public int Ukprn { get; set; }

        public string ProviderName { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public string ApprenticeshipMarketingInfo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactUsUrl { get; set; }

        public string ApprenticeshipInfoUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public double? LearnerSatisfaction { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public double? EmployerSatisfaction { get; set; }

        public string[] DeliveryModes { get; set; }

        public string Website { get; set; }

        [Nested]
        public IEnumerable<TrainingLocation> TrainingLocations { get; set; }

        [GeoPoint]
        public IEnumerable<GeoCoordinate> LocationPoints { get; set; }
    }
}