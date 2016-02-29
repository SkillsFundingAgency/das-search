using Sfa.Das.ApplicationServices.Models;
using System.Threading.Tasks;

namespace Sfa.Das.ApplicationServices
{
    public interface IProviderSearchService
    {
        Task<ProviderSearchResults> SearchByPostCode(int standardId, string postCode);
    }
}
