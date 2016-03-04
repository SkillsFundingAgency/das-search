namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Core.Models.ProviderImport;

    public interface IImportProviders
    {
        Task<IEnumerable<ProviderImport>> GetProviders();
    }
}