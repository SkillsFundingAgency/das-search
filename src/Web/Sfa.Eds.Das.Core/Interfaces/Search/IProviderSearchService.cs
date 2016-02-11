using Sfa.Eds.Das.Core.Models;

namespace Sfa.Eds.Das.Core.Search
{
    public interface IProviderSearchService
    {
        ProviderSearchResults SearchByStandardId(string standardId, int skip, int take);
    }
}