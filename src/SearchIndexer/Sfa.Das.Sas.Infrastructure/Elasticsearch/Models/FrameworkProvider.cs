using Nest;
using Newtonsoft.Json;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using System.Collections.Generic;

    public sealed class FrameworkProvider : IProviderAppreticeshipDocument
    {
        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string FrameworkId { get; set; }

        public int Level { get; set; }

        public int Ukprn { get; set; }

        public string ProviderName { get; set; }

        public string ProviderMarketingInfo { get; set; }

        public string ApprenticeshipMarketingInfo { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string Phone { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string Email { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string ContactUsUrl { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string ApprenticeshipInfoUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public double? LearnerSatisfaction { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public double? EmployerSatisfaction { get; set; }

        public string[] DeliveryModes { get; set; }

        [String(Index = FieldIndexOption.NotAnalyzed)]
        public string Website { get; set; }

        [Nested]
        public IEnumerable<TrainingLocation> TrainingLocations { get; set; }

        [GeoPoint]
        public IEnumerable<GeoCoordinate> LocationPoints { get; set; }

        public double? OverallAchievementRate { get; set; }

        public double? NationalOverallAchievementRate { get; set; }

        public string OverallCohort { get; set; }
    }
}
