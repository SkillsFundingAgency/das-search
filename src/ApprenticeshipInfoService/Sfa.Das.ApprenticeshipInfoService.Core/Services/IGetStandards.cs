namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using System.Collections.Generic;
    using SFA.DAS.Apprenticeships.Api.Types;

    public interface IGetStandards
    {
        IEnumerable<StandardSummary> GetAllStandards();

        Standard GetStandardById(string id);
    }
}
