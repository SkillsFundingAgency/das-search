namespace Sfa.Das.Web.Services
{
    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.ViewModels;

    public interface IProviderViewModelFactory
    {
        ProviderViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria);
    }
}