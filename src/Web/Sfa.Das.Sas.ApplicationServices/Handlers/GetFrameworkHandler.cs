using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetFrameworkHandler : IRequestHandler<GetFrameworkQuery, GetFrameworkResponse>
    {
        private readonly IGetFrameworks _getFrameworks;

        public GetFrameworkHandler(IGetFrameworks getFrameworks)
        {
            _getFrameworks = getFrameworks;
        }

        public GetFrameworkResponse Handle(GetFrameworkQuery message)
        {
            var response = new GetFrameworkResponse();

            if (message.Id.Length < 2)
            {
                response.StatusCode = GetFrameworkResponse.ResponseCodes.InvalidFrameworkId;

                return response;
            }

            var framework = _getFrameworks.GetFrameworkById(message.Id);

            if (framework == null)
            {
                response.StatusCode = GetFrameworkResponse.ResponseCodes.FrameworkNotFound;

                return response;
            }

            response.Framework = framework;
            response.SearchTerms = message.Keywords;

            return response;
        }
    }
}
