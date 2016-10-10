using System;
using System.Collections.Generic;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public interface IProviderApiClient : IDisposable
    {
        IEnumerable<Provider> Get(int providerUkprn);
    }
}