using System;
using SFA.DAS.Apprenticeships.Api.Client.Models;

namespace SFA.DAS.Apprenticeships.Api.Client
{
    public interface IFrameworkApiClient : IDisposable
    {
        Framework Get(int frameworkId);
    }
}