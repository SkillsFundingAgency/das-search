using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Types.Providers;


namespace Sfa.Das.Sas.Core.Domain.Repositories
{
    public interface IProviderRepository
    {
        Dictionary<long, string> GetProviderList();
        Provider GetProviderDetails(long prn);
    }
}