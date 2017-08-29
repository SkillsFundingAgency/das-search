using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IGetFrameworks
    {
        Framework GetFrameworkById(string id);

        List<Framework> GetAllFrameworks();

        int GetFrameworksAmount();

        long GetFrameworksOffer();

        int GetFrameworksExpiringSoon(int daysToExpire);
    }
}
