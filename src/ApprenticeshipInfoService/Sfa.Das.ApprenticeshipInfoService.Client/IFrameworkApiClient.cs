using System;
using Sfa.Das.ApprenticeshipInfoService.Client.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Client
{
    public interface IFrameworkApiClient : IDisposable
    {
        Framework Get(int frameworkId);
    }
}