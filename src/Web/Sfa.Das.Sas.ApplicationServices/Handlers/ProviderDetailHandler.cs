using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using MediatR;
    using Queries;
    using Responses;
    using SFA.DAS.Apprenticeships.Api.Types.Exceptions;

    public class ProviderDetailHandler : IAsyncRequestHandler<ProviderDetailQuery, ProviderDetailResponse>
    {
        private readonly IGetProviderDetails _getProviders;

        public ProviderDetailHandler(IGetProviderDetails getProviders)
        {
            _getProviders = getProviders;
        }

        public async Task<ProviderDetailResponse> Handle(ProviderDetailQuery message)
        {
            try
            {
                var provider = await _getProviders.GetProviderDetails(message.UkPrn);
                return new ProviderDetailResponse
                {
                    Provider = provider,
                    StatusCode = ProviderDetailResponse.ResponseCodes.Success
                };
            }
            catch (EntityNotFoundException)
            {
                return new ProviderDetailResponse
                {
                    StatusCode = ProviderDetailResponse.ResponseCodes.ProviderNotFound
                };
            }
            catch (HttpRequestException)
            {
                return new ProviderDetailResponse
                {
                    StatusCode = ProviderDetailResponse.ResponseCodes.HttpRequestException
                };
            }
        }
    }
}
