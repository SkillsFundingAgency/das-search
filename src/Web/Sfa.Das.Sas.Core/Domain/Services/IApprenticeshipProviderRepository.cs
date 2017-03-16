using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IApprenticeshipProviderRepository
    {
        ApprenticeshipDetails GetCourseByStandardCode(int ukprn, int locationId, string standardCode, bool hasNonLevyContract);
        ApprenticeshipDetails GetCourseByFrameworkId(int ukprn, int locationId, string frameworkId, bool hasNonLevyContract);
        int GetFrameworksAmountWithProviders();
        int GetStandardsAmountWithProviders();
    }
}
