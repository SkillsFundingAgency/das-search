using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Interfaces
{
    public interface IProviderNameSearchService
    {
        Task<ProviderNameSearchResultsAndPagination> SearchProviderNameAndAliases(string searchTerm, int page);
    }
}