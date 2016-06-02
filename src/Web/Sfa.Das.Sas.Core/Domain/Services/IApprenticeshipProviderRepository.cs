using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IApprenticeshipProviderRepository
    {
        ApprenticeshipDetails GetCourseByStandardCode(string providerid, string locationId, string standardCode);
        ApprenticeshipDetails GetCourseByFrameworkId(string providerid, string locationId, string frameworkId);
    }
}
