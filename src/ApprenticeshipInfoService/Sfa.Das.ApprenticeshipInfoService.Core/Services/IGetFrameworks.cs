namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using System.Collections.Generic;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IGetFrameworks
    {
        Framework GetFrameworkById(int id);
    }
}
