namespace Sfa.Eds.Das.Indexer.Core
{
    using System.Collections.Generic;

    public interface IGetActiveProviders
    {
        IEnumerable<int> GetActiveProviders();
    }
}