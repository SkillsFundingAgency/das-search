using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IGetProviders
    {
        long GetProvidersAmount();
        ProviderSearchResult GetProvidersFromSearch(int page, int take, string search);

    }
}