using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IProviderViewModelFactory
    {
        ApprenticeshipDetailsViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria);
    }
}