using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Linq;

    using FluentValidation;

    using Sfa.Das.Sas.ApplicationServices.Validators;

    public class GetFrameworkHandler : IRequestHandler<GetFrameworkQuery, GetFrameworkResponse>
    {
        private readonly IGetFrameworks _getFrameworks;

        private readonly AbstractValidator<GetFrameworkQuery> _validator;

        public GetFrameworkHandler(
            IGetFrameworks getFrameworks,
            AbstractValidator<GetFrameworkQuery> validator
            )
        {
            _getFrameworks = getFrameworks;
            _validator = validator;
        }

        public GetFrameworkResponse Handle(GetFrameworkQuery message)
        {
            var result = _validator.Validate(message);
            var response = new GetFrameworkResponse();

            if (!result.IsValid && result.Errors.Any(x => x.ErrorCode == ValidationCodes.InvalidId))
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
