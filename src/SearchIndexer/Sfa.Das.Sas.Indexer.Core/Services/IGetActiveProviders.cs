using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.Core.Services
{
    public interface IGetActiveProviders
    {
        IEnumerable<int> GetActiveProviders();
    }
}