namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class StubActiveProvidersClient : IActiveProviderClient
    {
        public async Task<IEnumerable<string>> GetProviders()
        {
            return ListOfProviders();
        }

        public IEnumerable<string> ListOfProviders()
        {
            yield return "10001309";
            yield return "10031241";
            yield return "10004355";
            yield return "10003347";
        }
    }
}