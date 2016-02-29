namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System.Collections.Generic;

    public class StubActiveProvidersClient : IActiveProviderClient
    {
        public IEnumerable<string> GetProviders()
        {
            yield return "10001309";
            yield return "10031241";
            yield return "10004355";
            yield return "10003347";
        }
    }
}