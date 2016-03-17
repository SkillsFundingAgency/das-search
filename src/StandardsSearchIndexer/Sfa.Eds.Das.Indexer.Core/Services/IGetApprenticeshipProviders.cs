namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Provider;

    public interface IGetApprenticeshipProviders
    {
        Task<IEnumerable<Provider>> GetApprenticeshipProvidersAsync();
    }
}