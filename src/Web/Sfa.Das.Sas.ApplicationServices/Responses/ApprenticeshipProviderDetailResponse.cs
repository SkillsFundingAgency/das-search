namespace Sfa.Das.Sas.ApplicationServices.Responses
{
    using SFA.DAS.Apprenticeships.Api.Types;
    using Sfa.Das.Sas.Core.Domain.Model;

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