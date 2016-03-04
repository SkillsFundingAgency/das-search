namespace Sfa.Eds.Das.Indexer.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Core.Models;

    public interface IGetProviders
    {
        Task<IEnumerable<Provider>> GetProviders();
    }
}