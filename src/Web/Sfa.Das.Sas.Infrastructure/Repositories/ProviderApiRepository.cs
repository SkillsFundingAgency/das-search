using Sfa.Das.Sas.Core.Domain.Services;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using SFA.DAS.Providers.Api.Client;

    public class ProviderApiRepository : IGetProviderDetails
    {

        private readonly IProviderApiClient _providerApiClient;

        public ProviderApiRepository(IProviderApiClient providerApiClient)
        {
            _providerApiClient = providerApiClient;
        }

        public async Task<Provider> GetProviderDetails(long ukPrn)
        {
            var result = await _providerApiClient.GetAsync(ukPrn);
            return result;
        }

        public IEnumerable<ProviderSummary> GetAllProviders()
        {
            var res = _providerApiClient.FindAll();
            return res;
        }
    }
}
