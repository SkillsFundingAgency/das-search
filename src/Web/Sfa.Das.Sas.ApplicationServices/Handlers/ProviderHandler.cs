using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Repositories;

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

            var response = new ProviderDetailResponse();

            var provider = await _providerDetailRepository.GetProviderDetails(message.ukPrn);

            response.Provider = provider;

            return response;

        }
    }
}
