using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Collections.Generic;

    public interface IProviderSearchService
    {
        Task<ProviderStandardSearchResults> SearchStandardProviders(string standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool nationalProviders, bool showAll, bool hasNonLevyContract);

        Task<ProviderFrameworkSearchResults> SearchFrameworkProviders(string frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool nationalProviders, bool showAll, bool hasNonLevyContract);
    }
}
