namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System.Collections.Generic;

    public interface IActiveProviderClient
    {
        IEnumerable<string> GetProviders();
    }
}