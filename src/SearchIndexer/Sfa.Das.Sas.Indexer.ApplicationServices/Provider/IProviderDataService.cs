using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Provider
{
    public interface IProviderDataService
    {
        Task<ICollection<Core.Models.Provider.Provider>> GetProviders();
    }
}