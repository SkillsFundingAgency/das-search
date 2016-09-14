namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using System.Collections.Generic;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IGetFrameworks
    {
        IEnumerable<FrameworkSummary> GetAllFrameworks();

        Framework GetFrameworkById(int id);
    }
}
