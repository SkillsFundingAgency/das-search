namespace Sfa.Eds.Das.Core.Domain.Services
{
    using Model;

    public interface IGetStandards
    {
        Standard GetStandardById(int id);
    }
}
