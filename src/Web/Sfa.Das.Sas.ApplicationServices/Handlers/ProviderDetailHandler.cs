namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core.Domain.Repositories;
    using MediatR;
    using Queries;
    using Responses;
    using SFA.DAS.Apprenticeships.Api.Types.Exceptions;

    public class ProviderDetailHandler : IAsyncRequestHandler<ProviderDetailQuery, ProviderDetailResponse>
    {
        private readonly IProviderDetailRepository _providerDetailRepository;

        public ProviderDetailHandler(IProviderDetailRepository providerDetailRepository)
        {
            _providerDetailRepository = providerDetailRepository;
        }

        public async Task<ProviderDetailResponse> Handle(ProviderDetailQuery message)
        {
            try
            {
                var provider = await _providerDetailRepository.GetProviderDetails(message.UkPrn);
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
