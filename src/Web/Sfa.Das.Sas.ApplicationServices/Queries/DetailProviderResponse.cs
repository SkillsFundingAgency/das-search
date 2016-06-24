namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Domain.Model;

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

        public string ApprenticeshipNameWithLevel { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public bool IsShortlisted { get; set; }
    }
}