using System.Collections.Generic;
using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface IProviderNameSearchMapping
    {
        ProviderNameSearchResultsAndPagination Map(SFADASApprenticeshipsApiTypesV3ProviderSearchResults document, string searchTerm);
    }
}
