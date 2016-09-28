namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    public class DetailProviderResponse
    {
        public enum ResponseCodes
        {
            Success,
            ApprenticeshipProviderNotFound,
            InvalidInput
        }

        public ResponseCodes StatusCode { get; set; }

        public ApprenticeshipDetails ApprenticeshipDetails { get; set; }

        public ApprenticeshipTrainingType ApprenticeshipType { get; set; }

        public string ApprenticeshipName { get; set; }

        public string ApprenticeshipLevel { get; set; }
    }
}
