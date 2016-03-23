namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sfa.Eds.Das.Indexer.Core.Models.Provider;

    public interface IMaintainProviderIndex : IMaintainSearchIndexes
    {
        Task IndexEntries(string indexName, ICollection<Provider> entries);
    }
}