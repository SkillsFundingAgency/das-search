using Nest;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models
{
    using System.Collections.Generic;

    public interface IProviderAppreticeshipDocument
    {
        int Ukprn { get; set; }
        string ProviderName { get; set; }
        string Id { get; set; }
        string ProviderMarketingInfo { get; set; }
        string ApprenticeshipMarketingInfo { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string ContactUsUrl { get; set; }
        string ApprenticeshipInfoUrl { get; set; }
        double? LearnerSatisfaction { get; set; }
        double? EmployerSatisfaction { get; set; }
        string[] DeliveryModes { get; set; }
        string Website { get; set; }
        IEnumerable<TrainingLocation> TrainingLocations { get; set; }
        IEnumerable<GeoCoordinate> LocationPoints { get; set; }
    }
}
