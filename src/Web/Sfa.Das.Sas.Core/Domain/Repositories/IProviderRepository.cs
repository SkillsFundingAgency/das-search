using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types.Providers;


namespace Sfa.Das.Sas.Core.Domain.Repositories
{
    public interface IProviderRepository
    {
        Task<Dictionary<long, string>> GetProviderList();
        Provider GetProviderDetails(long prn);
    }
}