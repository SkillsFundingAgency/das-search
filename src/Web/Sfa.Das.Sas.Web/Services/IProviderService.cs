using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IProviderService
    {
        Dictionary<long, string> GetProviderList();
        Provider GetProviderDetails(long prn);
    }
}