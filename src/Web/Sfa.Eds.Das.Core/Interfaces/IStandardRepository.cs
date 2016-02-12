namespace Sfa.Eds.Das.Core.Interfaces.Search
{
    using Sfa.Eds.Das.Core.Models;

    public interface IStandardRepository
    {
        Standard GetById(string id);
    }
}
