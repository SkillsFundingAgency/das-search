namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using System.Collections.Generic;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IGetStandards
    {
        IEnumerable<StandardSummary> GetAllStandards();

        Standard GetStandardById(int id);
    }
}
