using System.Collections.Generic;

namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IGetProviders
    {
        List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double lat, double lon);
    }
}
