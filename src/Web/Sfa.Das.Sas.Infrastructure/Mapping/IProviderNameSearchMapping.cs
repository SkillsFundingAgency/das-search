using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface IProviderNameSearchMapping
    {
        IEnumerable<ProviderNameSearchResult> FilterNonMatchingAliases(string searchTerm, IEnumerable<ProviderNameSearchResult> resultsToFilter);
    }
}
