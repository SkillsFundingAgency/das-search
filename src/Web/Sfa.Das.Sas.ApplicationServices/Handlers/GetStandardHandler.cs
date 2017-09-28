using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.AssessmentOrgs.Api.Client;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using Core.Domain.Services;
    using MediatR;
    using Queries;
    using Responses;

    public class GetStandardHandler : IRequestHandler<GetStandardQuery, GetStandardResponse>
    {
        private readonly IGetStandards _getStandards;
        private readonly IAssessmentOrgsApiClient _getAssessmentOrgs;

        public GetStandardHandler(IGetStandards getStandards, IAssessmentOrgsApiClient getAssessmentOrgs)
        {
            _getStandards = getStandards;
            _getAssessmentOrgs = getAssessmentOrgs;
        }

        public GetStandardResponse Handle(GetStandardQuery message)
        {
            var response = new GetStandardResponse();

            var standard = _getStandards.GetStandardById(message.Id);

            int intId;
            int.TryParse(message.Id, out intId);
            if (intId < 0)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.InvalidStandardId;
                return response;
            }

            if (standard == null)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.StandardNotFound;
                return response;
            }

            response.Standard = standard;
            response.SearchTerms = message.Keywords;

            try
            {
                var assessmentOrganisations = _getAssessmentOrgs.ByStandard(standard.StandardId);
                response.AssessmentOrganisations = assessmentOrganisations?.ToList() ?? new List<Organisation>();
            }
            catch (EntityNotFoundException)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.AssessmentOrgsEntityNotFound;
                return response;
            }
            catch (HttpRequestException)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.HttpRequestException;
                return response;
            }

            return response;
        }
    }
}
