using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetStandardProvidersHandler : IRequestHandler<GetStandardProvidersQuery, GetStandardProvidersResponse>
    {
        private readonly IGetStandards _getStandards;

        public GetStandardProvidersHandler(IGetStandards getStandards)
        {
            _getStandards = getStandards;
        }

        public GetStandardProvidersResponse Handle(GetStandardProvidersQuery message)
        {
            var standard = _getStandards.GetStandardById(message.StandardId);

            if (standard == null)
            {
                return new GetStandardProvidersResponse
                {
                    StatusCode = GetStandardProvidersResponse.ResponseCodes.NoStandardFound
                };
            }

            return new GetStandardProvidersResponse
            {
                StandardId = standard.StandardId,
                Title = standard.Title + ", level " + standard.Level,
                Keywords = message.Keywords,
                Postcode = message.Postcode,
                Level = standard.Level
            };
        }
    }
}
