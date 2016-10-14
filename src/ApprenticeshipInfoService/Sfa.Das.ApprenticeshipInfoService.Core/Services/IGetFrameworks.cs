namespace Sfa.Das.ApprenticeshipInfoService.Core.Services
{
    using System.Collections.Generic;
    using SFA.DAS.Apprenticeships.Api.Types;

    public interface IGetFrameworks
    {
        IEnumerable<FrameworkSummary> GetAllFrameworks();

        Framework GetFrameworkById(string id);
    }
}
