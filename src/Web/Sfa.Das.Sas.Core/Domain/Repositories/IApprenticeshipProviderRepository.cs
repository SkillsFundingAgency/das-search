using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Repositories
{
    public interface IApprenticeshipProviderRepository
    {
        ApprenticeshipDetails GetCourseByStandardCode(int ukprn, int locationId, string standardCode);
        ApprenticeshipDetails GetCourseByFrameworkId(int ukprn, int locationId, string frameworkId);
        int GetFrameworksAmountWithProviders();
        int GetStandardsAmountWithProviders();
    }
}
