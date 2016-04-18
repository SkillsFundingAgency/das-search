using Nest;

namespace Sfa.Infrastructure.Elasticsearch.Models
{
    public interface IProviderAppreticeshipDocument
    {
        int Ukprn { get; set; }
        string Name { get; set; }
        string Id { get; set; }
        int LocationId { get; set; }
        string LocationName { get; set; }
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
        Address Address { get; set; }
        [GeoPoint]
        GeoCoordinate LocationPoint { get; set; }
        [GeoShape]
        CircleGeoShape Location { get; set; }
    }
}
