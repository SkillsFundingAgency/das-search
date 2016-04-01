namespace Sfa.Eds.Das.Core.Domain.Services
{
    using Model;

    public interface IApprenticeshipRepository
    {
        Standard GetStandardById(int id);

        Framework GetFrameworkById(int id);
    }
}
