namespace Sfa.Eds.Das.Core.Domain.Services
{
    using Model;

    public interface IStandardRepository
    {
        Standard GetById(string id);
    }
}
