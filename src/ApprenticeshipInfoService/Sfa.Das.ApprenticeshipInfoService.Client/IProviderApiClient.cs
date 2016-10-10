using System;
using System.Collections.Generic;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Client
{
    public interface IProviderApiClient : IDisposable
    {
        IEnumerable<Provider> Get(int providerUkprn);
    }
}