using System;
using Sfa.Das.ApprenticeshipInfoService.Client.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Client
{
    public interface IApprenticeshipApiClient : IDisposable
    {
        Framework GetFramework(int frameworkId);
        Standard GetStandard(int standardCode);
    }
}