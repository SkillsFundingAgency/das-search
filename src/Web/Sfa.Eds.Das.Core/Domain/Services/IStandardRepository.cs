namespace Sfa.Eds.Das.Core.Domain.Services
{
    using Model;

    public interface IStandardRepository
    {
        Standard GetById(int id);

        Framework GetFrameworkById(int id);
    }
}
