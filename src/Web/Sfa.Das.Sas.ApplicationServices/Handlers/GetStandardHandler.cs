using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.AssessmentOrgs.Api.Client;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using Core.Domain.Services;
    using MediatR;
    using Queries;
    using Responses;
    using SFA.DAS.NLog.Logger;

    public class GetStandardHandler : IRequestHandler<GetStandardQuery, GetStandardResponse>
    {
        private readonly IGetStandards _getStandards;
        private readonly IAssessmentOrgsApiClient _getAssessmentOrgs;
        private readonly ILog _logger;

        public GetStandardHandler(
            IGetStandards getStandards, 
            IAssessmentOrgsApiClient getAssessmentOrgs,
            ILog logger)
        {
            _getStandards = getStandards;
            _getAssessmentOrgs = getAssessmentOrgs;
            _logger = logger;
        }

        public async Task<GetStandardResponse> Handle(GetStandardQuery message, CancellationToken cancellationToken)
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

            if (!standard.IsActiveStandard)
            {
                response.StatusCode = GetStandardResponse.ResponseCodes.Gone;
                return response;
            }

            response.Standard = standard;
            response.SearchTerms = message.Keywords;

            try
            {
                var assessmentOrganisations = _getAssessmentOrgs.ByStandard(standard.StandardId);
                response.AssessmentOrganisations = assessmentOrganisations?.ToList() ?? new List<Organisation>();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.Warn(ex, $"{typeof(EntityNotFoundException)} when trying to get assesment org by standard id");
                response.StatusCode = GetStandardResponse.ResponseCodes.AssessmentOrgsEntityNotFound;
                response.AssessmentOrganisations = new List<Organisation>();
            }
            catch (HttpRequestException ex)
            {
                _logger.Warn(ex, $"{typeof(HttpRequestException)} when trying to get assesment org by standard id");
                response.StatusCode = GetStandardResponse.ResponseCodes.HttpRequestException;
            }

            return response;
        }
    }
}
