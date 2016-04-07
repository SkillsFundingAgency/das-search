namespace Sfa.Eds.Das.Core.Domain.Services
{
    using Sfa.Eds.Das.Core.Domain.Model;

    public interface IApprenticeshipProviderRepository
    {
        Provider GetByStandardCode(string providerid, string locationId, string standardCode);
        Provider GetByFrameworkId(string providerid, string locationId, string frameworkId);
    }
}
