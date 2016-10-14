using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IGetFrameworks
    {
        Framework GetFrameworkById(string id);
        long GetFrameworksAmount();
        int GetFrameworksExpiringSoon();
    }
}
