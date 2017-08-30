using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Repositories;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ProviderHandler : IAsyncRequestHandler<ProviderDetailQuery, ProviderDetailResponse>
    {
        private readonly IProviderDetailRepository _providerDetailRepository;

        public ProviderHandler(IProviderDetailRepository providerDetailRepository)
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
            catch (EntityNotFoundException ex)
            {
                return new ProviderDetailResponse
                {
                    StatusCode = ProviderDetailResponse.ResponseCodes.ProviderNotFound
                };
            }
            catch (HttpRequestException ex)
            {
                return new ProviderDetailResponse
                {
                    StatusCode = ProviderDetailResponse.ResponseCodes.UkPrnNotCorrectLength
                };
            }
        }
    }
}
