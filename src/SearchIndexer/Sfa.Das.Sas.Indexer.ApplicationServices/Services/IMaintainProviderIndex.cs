using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Services
{
    public interface IMaintainProviderIndex : IMaintainSearchIndexes
    {
        Task IndexEntries(string indexName, ICollection<Core.Models.Provider.Provider> entries);
    }
}