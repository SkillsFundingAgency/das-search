using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IApprenticeshipProviderRepository
    {
        //DetailProviderResponse GetStandardProviderDetails(int ukprn, int locationId, string standardCode);

        ApprenticeshipDetails GetCourseByStandardCode(int ukprn, int locationId, string standardCode);
        ApprenticeshipDetails GetCourseByFrameworkId(int ukprn, int locationId, string frameworkId);
    }
}
