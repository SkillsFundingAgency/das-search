using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Types;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public interface IProviderApiClient : IDisposable
    {
        IEnumerable<Provider> Get(int providerUkprn);
    }
}