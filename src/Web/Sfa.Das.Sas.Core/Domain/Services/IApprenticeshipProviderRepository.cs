using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IApprenticeshipProviderRepository
    {
        Provider GetByStandardCode(string providerid, string locationId, string standardCode);
        Provider GetByFrameworkId(string providerid, string locationId, string frameworkId);
    }
}
