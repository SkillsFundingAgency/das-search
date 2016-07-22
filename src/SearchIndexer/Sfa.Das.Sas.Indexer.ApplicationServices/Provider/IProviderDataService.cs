using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Provider
{
    public interface IProviderDataService
    {
        Task<ICollection<Core.Models.Provider.Provider>> GetProviders();
    }
}