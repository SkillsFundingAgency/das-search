using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;

namespace Sfa.Das.Sas.Indexer.Core.Services
{
    public interface IGetApprenticeshipProviders
    {
        Task<IEnumerable<Provider>> GetApprenticeshipProvidersAsync();
    }
}