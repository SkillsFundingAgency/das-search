using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices
{
    public interface IProviderSearchService
    {
        Task<ProviderStandardSearchResults> SearchByStandardPostCode(int standardId, string postCode);
        Task<ProviderFrameworkSearchResults> SearchByFrameworkPostCode(int frameworkId, string postCode);
    }
}
