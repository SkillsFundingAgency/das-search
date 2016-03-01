using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;

namespace Sfa.Das.ApplicationServices
{
    public interface IProviderSearchService
    {
        Task<ProviderSearchResults> SearchByPostCode(int standardId, string postCode);
    }
}
