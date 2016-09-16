namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IGetProviders
    {
        StandardProviderSearchResultsItem GetByStandardIdAndLocation(int id);
    }
}
