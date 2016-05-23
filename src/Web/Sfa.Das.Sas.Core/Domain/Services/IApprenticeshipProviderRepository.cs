using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IApprenticeshipProviderRepository
    {
        ProviderCourse GetCourseByStandardCode(string providerid, string locationId, string standardCode);
        ProviderCourse GetCourseByFrameworkId(string providerid, string locationId, string frameworkId);
    }
}
