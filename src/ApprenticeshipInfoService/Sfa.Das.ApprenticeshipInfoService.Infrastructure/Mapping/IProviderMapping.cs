using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping
{
    public interface IProviderMapping
    {
        ApprenticeshipDetails MapToProvider(StandardProviderSearchResultsItem item, int selectedLocationId);

        ApprenticeshipDetails MapToProvider(FrameworkProviderSearchResultsItem item, int selectedLocationId);
    }
}
