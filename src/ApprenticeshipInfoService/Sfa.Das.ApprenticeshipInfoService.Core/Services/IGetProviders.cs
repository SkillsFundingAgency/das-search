namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using System.Collections.Generic;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models.Responses;
    using SFA.DAS.Apprenticeships.Api.Types;

    public interface IGetProviders
    {
        IEnumerable<Provider> GetAllProviders();

        IEnumerable<Provider> GetProvidersByUkprn(int ukprn);

        List<StandardProviderSearchResultsItemResponse> GetByStandardIdAndLocation(int id, double lat, double lon, int page);

        List<FrameworkProviderSearchResultsItemResponse> GetByFrameworkIdAndLocation(int id, double lat, double lon, int page);
    }
}
