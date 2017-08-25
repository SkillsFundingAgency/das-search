namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Domain.Repositories;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using SFA.DAS.Providers.Api.Client;

    public class ProviderDetailRepository : IProviderDetailRepository
    {

        private readonly IProviderApiClient _providerApiClient;

        public ProviderDetailRepository(IProviderApiClient providerApiClient)
        {
            _providerApiClient = providerApiClient;
        }

        public Provider GetProviderDetails(long prn)
        {
            return _providerApiClient.Get(prn);
        }

        public async Task<Dictionary<long, string>> GetProviderList()
        {
            var res = await _providerApiClient.FindAllAsync();
            return res.ToDictionary(x => x.Ukprn, x => x.ProviderName);
        }
    }
}
