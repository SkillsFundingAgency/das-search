using System.Threading.Tasks;
using Sfa.Das.ApplicationServices.Models;

namespace Sfa.Das.ApplicationServices
{
    public interface IProviderSearchService
    {
        Task<ProviderStandardSearchResults> SearchByStandardPostCode(int standardId, string postCode);
        Task<ProviderFrameworkSearchResults> SearchByFrameworkPostCode(int frameworkId, string postCode);
    }
}
