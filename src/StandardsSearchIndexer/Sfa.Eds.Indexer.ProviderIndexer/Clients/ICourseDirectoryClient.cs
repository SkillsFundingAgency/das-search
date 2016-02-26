namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Common.Models;

    public interface ICourseDirectoryClient
    {
        Task<IEnumerable<Provider>> GetProviders();
    }
}