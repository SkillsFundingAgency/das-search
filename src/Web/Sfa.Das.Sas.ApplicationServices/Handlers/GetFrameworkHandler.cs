using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetFrameworkHandler : RequestHandler<GetFrameworkQuery, GetFrameworkResponse>
    {
        private readonly IGetFrameworks _getFrameworks;

        private readonly AbstractValidator<GetFrameworkQuery> _validator;
        private readonly ILogger<GetFrameworkHandler> _logger;

        public GetFrameworkHandler(
            IGetFrameworks getFrameworks,
            AbstractValidator<GetFrameworkQuery> validator,
            ILogger<GetFrameworkHandler> logger)
        {
            _getFrameworks = getFrameworks;
            _validator = validator;
            _logger = logger;
        }

        protected override GetFrameworkResponse Handle(GetFrameworkQuery message)
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
                _logger.LogWarning($"Not possible to get framework {message.Id}");
                response.StatusCode = GetFrameworkResponse.ResponseCodes.FrameworkNotFound;

                return response;
            }

            if (!framework.IsActiveFramework)
            {
                _logger.LogWarning($"Framework {message.Id} is not active");

                response.StatusCode = GetFrameworkResponse.ResponseCodes.Gone;

                return response;
            }

            response.Framework = framework;
            response.SearchTerms = message.Keywords;

            return response;
        }
    }
}
