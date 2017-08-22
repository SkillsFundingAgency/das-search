using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.DAS.Providers.Api.Client;

    public class ProviderService : IProviderService
    {

        private readonly IProviderApiClient _providerApiClient;

        public ProviderService(IProviderApiClient providerApiClient)
        {
            _providerApiClient = providerApiClient;
        }

        public Dictionary<long, string> GetProviderList()
        {
            var res = _providerApiClient.FindAll()
                .ToDictionary(x => x.Ukprn, x => x.ProviderName);

            return res;
        }

        public Provider GetProviderDetails(long prn)
        {
            return _providerApiClient.Get(prn);
        }
    }
}
