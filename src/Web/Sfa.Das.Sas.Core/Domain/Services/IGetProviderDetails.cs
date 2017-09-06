using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Core.Domain.Services
{
    public interface IGetProviderDetails
    {
        IEnumerable<ProviderSummary> GetAllProviders();
        Task<Provider> GetProviderDetails(long ukPrn);
    }
}