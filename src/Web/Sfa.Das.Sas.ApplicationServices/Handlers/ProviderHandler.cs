using System;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Repositories;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class ProviderHandler : IRequestHandler<ProviderQuery, ProviderDetailResponse>
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public ProviderDetailResponse Handle(ProviderQuery message)
        {

            var response = new ProviderDetailResponse();

            var provider = _providerRepository.GetProviderDetails(message.Prn);

            response.Provider = provider;

            return response;

        }
    }
}
