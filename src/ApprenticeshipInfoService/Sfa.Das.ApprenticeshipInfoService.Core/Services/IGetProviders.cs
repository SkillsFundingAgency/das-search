using System.Collections.Generic;

namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IGetProviders
    {
        IEnumerable<Provider> GetAllProviders();

        IEnumerable<Provider> GetProvidersByUkprn(int ukprn);

        List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double lat, double lon, int page);

        List<FrameworkProviderSearchResultsItem> GetByFrameworkIdAndLocation(int id, double lat, double lon, int page);
    }
}
