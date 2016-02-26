namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IActiveProviderClient
    {
        Task<IEnumerable<string>> GetProviders();
    }
}