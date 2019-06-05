using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.Shared.Components.ViewModels
{
    public class TrainingProviderDetailQueryViewModel
    {
        public long Ukprn { get; set; }
        public int Page { get; set; }
        public string ApprenticeshipId { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
    }
}