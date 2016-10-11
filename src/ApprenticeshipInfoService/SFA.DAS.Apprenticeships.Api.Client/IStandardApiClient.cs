using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Client.Models;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public interface IStandardApiClient : IDisposable
    {
        Standard Get(int standardCode);
        IEnumerable<StandardSummary> FindAll();
    }
}