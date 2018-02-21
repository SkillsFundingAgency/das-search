using System;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices
{
    public class ProviderNameSearchService : IProviderNameSearchService
    {
   Task<ProviderNameSearchResults> IProviderNameSearchService.SearchProviderNameAndAliases(string searchTerm, int page, int take)
        {
            throw new NotImplementedException();
        }
    }
}
