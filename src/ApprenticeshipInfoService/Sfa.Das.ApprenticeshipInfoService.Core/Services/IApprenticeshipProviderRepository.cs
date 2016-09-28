namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IApprenticeshipProviderRepository
    {
        ApprenticeshipDetails GetCourseByStandardCode(int ukprn, int locationId, string standardCode);

        ApprenticeshipDetails GetCourseByFrameworkId(int ukprn, int locationId, string frameworkId);
    }
}
