using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IProviderViewModelFactory
    {
        ProviderViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria);
    }
}