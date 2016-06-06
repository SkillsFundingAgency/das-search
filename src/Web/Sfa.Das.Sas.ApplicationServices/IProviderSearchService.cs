using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IProviderSearchService
    {
        Task<ProviderStandardSearchResults> SearchStandardProviders(int standardId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes, bool showAll);
        Task<ProviderFrameworkSearchResults> SearchByFrameworkPostCode(int frameworkId, string postCode, Pagination pagination, IEnumerable<string> deliveryModes);
    }
}
