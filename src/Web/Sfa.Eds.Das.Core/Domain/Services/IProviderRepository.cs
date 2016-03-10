namespace Sfa.Eds.Das.Core.Domain.Services
{
    using Sfa.Eds.Das.Core.Domain.Model;

    public interface IProviderRepository
    {
        Provider GetById(string providerid, string locationId, string standardCode);
    }
}
