namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Collections.Generic;

    public interface IGetActiveProviders
    {
        IEnumerable<int> GetActiveProviders();
    }
}