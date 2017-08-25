using System;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Repositories;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ProviderHandler : IRequestHandler<ProviderDetailQuery, ProviderDetailResponse>
    {
        private readonly IProviderDetailRepository _providerDetailRepository;

        public ProviderHandler(IProviderDetailRepository providerDetailRepository)
        {
            _providerDetailRepository = providerDetailRepository;
        }

        public ProviderDetailResponse Handle(ProviderDetailQuery message)
        {

            var response = new ProviderDetailResponse();

            var provider = _providerDetailRepository.GetProviderDetails(message.Prn);

            response.Provider = provider;

            return response;

        }
    }
}
