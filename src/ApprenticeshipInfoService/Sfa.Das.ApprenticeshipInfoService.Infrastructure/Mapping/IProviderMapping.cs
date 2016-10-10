using Sfa.Das.ApprenticeshipInfoService.Core.Models;
using Sfa.Das.ApprenticeshipInfoService.Core.Models.Responses;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping
{
    public interface IProviderMapping
    {
        ApprenticeshipDetails MapToProvider(StandardProviderSearchResultsItem item, int selectedLocationId);

        ApprenticeshipDetails MapToProvider(FrameworkProviderSearchResultsItem item, int selectedLocationId);

        StandardProviderSearchResultsItemResponse MapToStandardProviderResponse(StandardProviderSearchResultsItem item);

        FrameworkProviderSearchResultsItemResponse MapToFrameworkProviderResponse(FrameworkProviderSearchResultsItem item);
    }
}
