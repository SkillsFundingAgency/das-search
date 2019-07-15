using Sfa.Das.Sas.Core.Domain;

namespace Sfa.Das.Sas.ApplicationServices.Responses
{
    using System.Collections.Generic;
    using Core.Domain.Model;
    using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;

    public class GetStandardResponse
    {
        public enum ResponseCodes
        {
            Success,
            InvalidStandardId,
            StandardNotFound,
            AssessmentOrgsEntityNotFound,
            HttpRequestException,
            Gone
        }

        public ResponseCodes StatusCode { get; set; }

        public Standard Standard { get; set; }

        public string SearchTerms { get; set; }

        public IEnumerable<AssessmentOrganisation> AssessmentOrganisations { get; set; }

    }
}
