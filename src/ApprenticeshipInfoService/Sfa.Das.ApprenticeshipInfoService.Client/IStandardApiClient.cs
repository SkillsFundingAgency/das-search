using System;
using Sfa.Das.ApprenticeshipInfoService.Client.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Client
{
    public interface IStandardApiClient : IDisposable
    {
        Standard Get(int standardCode);
    }
}