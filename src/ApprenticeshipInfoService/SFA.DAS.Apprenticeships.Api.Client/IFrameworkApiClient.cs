using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Client.Models;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public interface IFrameworkApiClient : IDisposable
    {
        Framework Get(string frameworkId);
        IEnumerable<FrameworkSummary> FindAll();
    }
}