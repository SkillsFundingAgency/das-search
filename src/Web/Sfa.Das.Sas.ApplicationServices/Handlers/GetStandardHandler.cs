namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;
    using Sfa.Das.Sas.ApplicationServices.Queries;
    using Sfa.Das.Sas.ApplicationServices.Responses;
    using Sfa.Das.Sas.Core.Domain.Services;
    using SFA.DAS.Apprenticeships.Api.Client;

    public class GetStandardHandler : IRequestHandler<GetStandardQuery, GetStandardResponse>
    {
        private readonly IGetStandards _getStandards;

        public GetStandardHandler(IGetStandards getStandards)
        {
            _getStandards = getStandards;
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

            response.AssessmentOrgansations = new List<Organisation>();

            response.Standard = standard;
            response.SearchTerms = message.Keywords;

            return response;
        }
    }
}
