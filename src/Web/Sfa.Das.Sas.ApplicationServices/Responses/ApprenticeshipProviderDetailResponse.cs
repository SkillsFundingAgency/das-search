using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Responses
{
    public class ApprenticeshipProviderDetailResponse
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